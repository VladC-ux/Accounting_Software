using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.Data.Entities;
using Accounting_Software.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Accounting_Software.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IUserRepository userRepo, IEmailService emailService)
        {
            _userRepo = userRepo;
            _emailService = emailService;
        }

        // ─── REGISTER ───────────────────────────────────────────────

        [HttpGet]
        public IActionResult Register()
        {
            if (_userRepo.UserCount() > 0)
                return RedirectToAction("Login");

            return View(new RegisterViewModel());
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (_userRepo.UserCount() > 0)
                return RedirectToAction("Login");

            if (!ModelState.IsValid)
                return View(model);

            if (_userRepo.GetByEmail(model.Email) != null)
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(model);
            }

            var (hash, salt) = HashPassword(model.Password);

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                Balance = 0m
            };

            _userRepo.Add(user);
            return RedirectToAction("Login", new { registered = true });
        }

        // ─── LOGIN ───────────────────────────────────────────────────

        [HttpGet]
        public IActionResult Login(bool registered = false, bool reset = false)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Dashboard");

            if (_userRepo.UserCount() == 0)
                return RedirectToAction("Register");

            // Existing user without password (migrated from old system)
            var existing = _userRepo.GetAll().FirstOrDefault();
            if (existing != null && string.IsNullOrEmpty(existing.PasswordHash))
                return RedirectToAction("SetupAuth");

            if (registered)
                ViewBag.Message = "Registration successful! Please log in.";
            if (reset)
                ViewBag.Message = "Password updated! Please log in.";

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _userRepo.GetByEmail(model.Email);
            if (user == null || !VerifyPassword(model.Password, user.PasswordHash!, user.PasswordSalt!))
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name ?? user.Email!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = model.RememberMe
                    ? DateTimeOffset.UtcNow.AddDays(30)
                    : DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
            return RedirectToAction("Index", "Dashboard");
        }

        // ─── SETUP AUTH (for existing users without password) ────────

        [HttpGet]
        public IActionResult SetupAuth()
        {
            var existing = _userRepo.GetAll().FirstOrDefault();
            if (existing == null) return RedirectToAction("Register");
            if (!string.IsNullOrEmpty(existing.PasswordHash)) return RedirectToAction("Login");
            return View(new RegisterViewModel { Name = existing.Name ?? "" });
        }

        [HttpPost]
        public IActionResult SetupAuth(RegisterViewModel model)
        {
            var existing = _userRepo.GetAll().FirstOrDefault();
            if (existing == null) return RedirectToAction("Register");
            if (!string.IsNullOrEmpty(existing.PasswordHash)) return RedirectToAction("Login");

            if (!ModelState.IsValid) return View(model);

            var (hash, salt) = HashPassword(model.Password);
            existing.Name = model.Name;
            existing.Email = model.Email;
            existing.PasswordHash = hash;
            existing.PasswordSalt = salt;
            _userRepo.Update(existing);

            return RedirectToAction("Login", new { registered = true });
        }

        // ─── LOGOUT ──────────────────────────────────────────────────

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // ─── FORGOT PASSWORD ─────────────────────────────────────────

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _userRepo.GetByEmail(model.Email);
            if (user != null)
            {
                var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
                user.PasswordResetToken = token;
                user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);
                _userRepo.Update(user);

                // Link goes to ConfirmReset — user must click "Approve" in their email
                var confirmLink = Url.Action("ConfirmReset", "Auth", new { token }, Request.Scheme);
                try
                {
                    await _emailService.SendPasswordResetEmailAsync(user.Email!, confirmLink!);
                    ViewBag.EmailSent = true;
                }
                catch
                {
                    // Do NOT reveal the link — show a configuration error instead
                    ViewBag.EmailError = true;
                }
            }
            else
            {
                // Same response whether email exists or not (prevent email enumeration)
                ViewBag.EmailSent = true;
            }

            return View(model);
        }

        // ─── CONFIRM RESET (approval step from email) ─────────────────

        [HttpGet]
        public IActionResult ConfirmReset(string token)
        {
            var user = _userRepo.GetAll().FirstOrDefault(u =>
                u.PasswordResetToken == token &&
                u.PasswordResetTokenExpiry > DateTime.UtcNow);

            if (user == null)
            {
                ViewBag.Invalid = true;
                return View();
            }

            ViewBag.Token = token;
            return View();
        }

        // ─── RESET PASSWORD ──────────────────────────────────────────

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            var user = _userRepo.GetAll().FirstOrDefault(u =>
                u.PasswordResetToken == token &&
                u.PasswordResetTokenExpiry > DateTime.UtcNow);

            if (user == null)
            {
                ViewBag.Invalid = true;
                return View(new ResetPasswordViewModel());
            }

            return View(new ResetPasswordViewModel { Token = token });
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _userRepo.GetAll().FirstOrDefault(u =>
                u.PasswordResetToken == model.Token &&
                u.PasswordResetTokenExpiry > DateTime.UtcNow);

            if (user == null)
            {
                ViewBag.Invalid = true;
                return View(model);
            }

            var (hash, salt) = HashPassword(model.NewPassword);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;
            _userRepo.Update(user);

            return RedirectToAction("Login", new { reset = true });
        }

        // ─── HELPERS ─────────────────────────────────────────────────

        private static (string hash, string salt) HashPassword(string password)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(16);
            string salt = Convert.ToBase64String(saltBytes);

            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32));

            return (hash, salt);
        }

        private static bool VerifyPassword(string password, string hash, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            string computed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32));

            return computed == hash;
        }
    }
}
