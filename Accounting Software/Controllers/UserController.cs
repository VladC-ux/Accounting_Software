
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

        public IActionResult ProductsTransaction(int id, DateTime? dateFrom, DateTime? dateTo, string? actionType)
        {
            var all = _transHistory.GetHistoryByUserId(id);

            if (dateFrom.HasValue)
                all = all.Where(t => t.SoldDate >= dateFrom.Value).ToList();

            if (dateTo.HasValue)
                all = all.Where(t => t.SoldDate <= dateTo.Value.AddDays(1).AddSeconds(-1)).ToList();

            if (!string.IsNullOrEmpty(actionType) && actionType != "All")
                all = all.Where(t => t.typeofAction == actionType).ToList();

            var model = new TransactionFilterViewModel
            {
                UserId = id,
                DateFrom = dateFrom,
                DateTo = dateTo,
                ActionType = actionType,
                Transactions = all
            };

            return View(model);
        }
    }
}
