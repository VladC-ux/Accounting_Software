using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    public class SoldListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
