using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    /// <summary>
    /// Переключение языка интерфейса. Сохраняет выбор в cookie на год.
    /// Формат чисел/дат держим в en-US, переключаем только UI-культуру.
    /// </summary>
    public class CultureController : Controller
    {
        private static readonly string[] Allowed = { "en-US", "hy-AM", "ru-RU" };

        [HttpGet]
        public IActionResult Set(string culture, string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(culture) || Array.IndexOf(Allowed, culture) < 0)
                culture = "en-US";

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(
                    new RequestCulture("en-US", culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                    Path = "/"
                });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction("Index", "Dashboard");
        }
    }
}
