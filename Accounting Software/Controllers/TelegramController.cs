using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Accounting_Software.Controllers
{
    [Authorize]
    public class TelegramController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly ITelegramNotifier _notifier;
        private readonly IConfiguration _config;

        public TelegramController(IUserRepository userRepo, ITelegramNotifier notifier, IConfiguration config)
        {
            _userRepo = userRepo;
            _notifier = notifier;
            _config = config;
        }

        public IActionResult Index()
        {
            var user = _userRepo.GetAll().FirstOrDefault();
            if (user == null) return RedirectToAction("Register", "Auth");

            var vm = new TelegramSettingsViewModel
            {
                IsLinked = user.TelegramChatId.HasValue,
                TelegramUsername = user.TelegramUsername,
                LinkCode = user.TelegramLinkCode,
                BotUsername = _config["Telegram:BotUsername"],
                NotifySales = user.TelegramNotifySales,
                NotifyLowStock = user.TelegramNotifyLowStock,
                DailyReport = user.TelegramDailyReport,
                BotConfigured = _notifier.IsConfigured
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult GenerateCode()
        {
            var user = _userRepo.GetAll().FirstOrDefault();
            if (user == null) return RedirectToAction("Index");

            user.TelegramLinkCode = GenerateShortCode();
            _userRepo.Update(user);

            TempData["Success"] = "Code generated. Send it to the bot using /link <code>.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Unlink()
        {
            var user = _userRepo.GetAll().FirstOrDefault();
            if (user == null) return RedirectToAction("Index");

            user.TelegramChatId = null;
            user.TelegramUsername = null;
            user.TelegramLinkCode = null;
            _userRepo.Update(user);

            TempData["Success"] = "Account unlinked from Telegram.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SavePreferences(bool notifySales, bool notifyLowStock, bool dailyReport)
        {
            var user = _userRepo.GetAll().FirstOrDefault();
            if (user == null) return RedirectToAction("Index");

            user.TelegramNotifySales = notifySales;
            user.TelegramNotifyLowStock = notifyLowStock;
            user.TelegramDailyReport = dailyReport;
            _userRepo.Update(user);

            TempData["Success"] = "Notification preferences saved.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SendTest()
        {
            var user = _userRepo.GetAll().FirstOrDefault();
            if (user?.TelegramChatId == null)
            {
                TempData["Error"] = "Link your account first.";
                return RedirectToAction("Index");
            }

            await _notifier.SendMessageAsync(user.TelegramChatId.Value,
                "✅ <b>Test message</b>\nIf you can read this — the bot works!");
            TempData["Success"] = "Test message sent to Telegram.";
            return RedirectToAction("Index");
        }

        private static string GenerateShortCode()
        {
            const string alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var bytes = RandomNumberGenerator.GetBytes(8);
            return new string(bytes.Select(b => alphabet[b % alphabet.Length]).ToArray());
        }
    }
}
