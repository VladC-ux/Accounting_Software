using Accounting_Software.Data.Entities;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITransactionHistoryService _transHistory;
        private readonly IUserRepository _userRepo;
        private readonly ITransactionHistoryRepository _transRepo;

        public UserController(IUserService userService, ITransactionHistoryService transactionHistory,
            IUserRepository userRepository, ITransactionHistoryRepository transactionHistoryRepository)
        {
            _userService = userService;
            _transHistory = transactionHistory;
            _userRepo = userRepository;
            _transRepo = transactionHistoryRepository;
        }

        public IActionResult Index(int Id = 0)
        {
            var user = Id > 0 ? _userRepo.GetUserById(Id) : _userRepo.GetAll().FirstOrDefault();
            if (user == null) return RedirectToAction("Index", "Dashboard");

            var transactions = _transHistory.GetHistoryByUserId(user.Id);

            ViewBag.TotalSpent      = transactions.Where(t => t.typeofAction == "Add").Sum(t => t.Total);
            ViewBag.TotalEarned     = transactions.Where(t => t.typeofAction == "Sale").Sum(t => t.Total);
            ViewBag.TotalDeposited  = transactions.Where(t => t.typeofAction == "Deposit").Sum(t => t.Price);
            ViewBag.RecentTx        = transactions.Take(6).ToList();
            ViewBag.Email           = user.Email;

            return View(_userService.GetBalance(user.Id));
        }

        [HttpPost]
        public IActionResult TopUp(decimal amount)
        {
            var user = _userRepo.GetAll().FirstOrDefault();
            if (user == null)
                return RedirectToAction("Index");

            if (amount <= 0)
            {
                TempData["Error"] = "Amount must be greater than zero.";
                return RedirectToAction("Index", new { Id = user.Id });
            }

            user.Balance += amount;
            _userRepo.Update(user);

            _transRepo.Add(new TransactionHistory
            {
                ProductName = "Balance Top-Up",
                Price = amount,
                Count = 1,
                Mass = 1,
                UserId = user.Id,
                typeofAction = "Deposit",
                SoldDate = DateTime.Now,
                Description = "Manual deposit"
            });

            TempData["Success"] = $"{amount:F2} added to your balance!";
            return RedirectToAction("Index", new { Id = user.Id });
        }

        public IActionResult ProductsTransaction(int id, DateTime? dateFrom, DateTime? dateTo, string? actionType)
        {
            if (id == 0)
            {
                var u = _userRepo.GetAll().FirstOrDefault();
                id = u?.Id ?? 0;
            }

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
