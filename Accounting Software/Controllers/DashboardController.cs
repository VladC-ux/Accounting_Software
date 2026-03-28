using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly ISellerService _sellerService;
        private readonly IProductService _productService;
        private readonly IStoreService _storeService;
        private readonly ITransactionHistoryService _transactionHistoryService;

        public DashboardController(
            IUserRepository userRepository,
            IUserService userService,
            ISellerService sellerService,
            IProductService productService,
            IStoreService storeService,
            ITransactionHistoryService transactionHistoryService)
        {
            _userRepository = userRepository;
            _userService = userService;
            _sellerService = sellerService;
            _productService = productService;
            _storeService = storeService;
            _transactionHistoryService = transactionHistoryService;
        }

        public IActionResult Index()
        {
            if (_userRepository.UserCount() == 0)
                return RedirectToAction("AddUser", "User");

            var users = _userRepository.GetAll();
            var user = users.First();

            var userViewModel = _userService.GetBalance(user.Id);
            var transactions = _transactionHistoryService.GetHistoryByUserId(user.Id);

            var dashboard = new DashboardViewModel
            {
                UserId = user.Id,
                UserName = user.Name ?? "User",
                Balance = userViewModel.Balance,
                TotalSellers = _sellerService.GetAll().Count,
                TotalProducts = _productService.GetAll().Count,
                TotalStores = _storeService.GetAll().Count,
                TotalSpent = transactions
                    .Where(t => t.typeofAction == "Add")
                    .Sum(t => t.Total),
                TotalEarned = transactions
                    .Where(t => t.typeofAction == "Sale")
                    .Sum(t => t.Total),
                LastTransactions = transactions.Take(5).ToList()
            };

            return View(dashboard);
        }
    }
}
