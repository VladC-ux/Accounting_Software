using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    public class WareHouseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
