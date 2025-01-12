using Accounting_Software.Service_Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index(int Id)
        {
            var data = _userService.GetBalance(Id);
            return View(data);
        }
    }
}
