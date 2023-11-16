using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
