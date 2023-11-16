using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    public class SellerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
