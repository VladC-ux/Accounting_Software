
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITransactionHistoryService _transHistory;
        private readonly IUserRepository _userRepo;

        public UserController(IUserService userService, ITransactionHistoryService transactionHistory,IUserRepository userRepository)
        {
            _userService = userService;
            _transHistory = transactionHistory;
            _userRepo = userRepository;
        }

        public IActionResult Index(int Id)
        {
            ViewBag.Users = _userRepo.GetAll();
            var data = _userService.GetBalance(Id);
            return View(data);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            var userCount = _userRepo.UserCount();
            if (userCount >= 1)
            {
                return RedirectToAction("Index", "Seller");
            }
            return View(new UserViewModel()); 
        }
        [HttpPost]
        public IActionResult AddUser(UserViewModel user)
        {      
            try
            {             
                _userService.Add(user);
                return RedirectToAction("Index", "Seller");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ошибка при добавлении: " + ex.Message;
                return View(user); 
            }
        }

        public IActionResult ProductsTransaction(int id)
        {
            var data =  _transHistory.GetHistoryByUserId(id);
            return View(data);
        }
    }
}
