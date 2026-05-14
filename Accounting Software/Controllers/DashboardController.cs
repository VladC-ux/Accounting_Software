using System.Globalization;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    [Authorize]
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
                return RedirectToAction("Register", "Auth");

            var users = _userRepository.GetAll();
            var user = users.First();

            var userViewModel = _userService.GetBalance(user.Id);
            var transactions = _transactionHistoryService.GetHistoryByUserId(user.Id);

            var now = DateTime.Now;
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var prevMonthStart = monthStart.AddMonths(-1);

            decimal SumByType(string type, DateTime from, DateTime to) => transactions
                .Where(t => t.typeofAction == type && t.SoldDate >= from && t.SoldDate < to)
                .Sum(t => t.Total);

            decimal SumDeposit(DateTime from, DateTime to) => transactions
                .Where(t => t.typeofAction == "Deposit" && t.SoldDate >= from && t.SoldDate < to)
                .Sum(t => t.Price);

            var totalSpent = transactions.Where(t => t.typeofAction == "Add").Sum(t => t.Total);
            var totalEarned = transactions.Where(t => t.typeofAction == "Sale").Sum(t => t.Total);
            var totalDeposited = transactions.Where(t => t.typeofAction == "Deposit").Sum(t => t.Price);

            var monthSpent = SumByType("Add", monthStart, monthStart.AddMonths(1));
            var monthEarned = SumByType("Sale", monthStart, monthStart.AddMonths(1));
            var prevMonthSpent = SumByType("Add", prevMonthStart, monthStart);
            var prevMonthEarned = SumByType("Sale", prevMonthStart, monthStart);

            var flow = new List<MonthlyFlowPoint>();
            var sparkProfit = new List<decimal>();
            var sparkIncome = new List<decimal>();
            var sparkExpense = new List<decimal>();
            var sparkBalance = new List<decimal>();

            var enCulture = new CultureInfo("en-US");
            decimal runningBalance = 0m;
            for (int i = 5; i >= 0; i--)
            {
                var from = monthStart.AddMonths(-i);
                var to = from.AddMonths(1);
                var inc = SumByType("Sale", from, to);
                var exp = SumByType("Add", from, to);
                var dep = SumDeposit(from, to);
                runningBalance += dep + inc - exp;
                flow.Add(new MonthlyFlowPoint
                {
                    Label = from.ToString("MMM", enCulture),
                    Income = inc,
                    Expense = exp
                });
                sparkProfit.Add(inc - exp);
                sparkIncome.Add(inc);
                sparkExpense.Add(exp);
                sparkBalance.Add(runningBalance);
            }

            var palette = new[] { "#2D6A4F", "#52B788", "#F4A261", "#E76F51", "#4A90E2", "#9CA3AF" };
            var purchaseGroups = transactions
                .Where(t => t.typeofAction == "Add")
                .GroupBy(t => string.IsNullOrWhiteSpace(t.ProductName) ? "Other" : t.ProductName)
                .Select(g => new { Name = g.Key, Amount = g.Sum(x => x.Total) })
                .OrderByDescending(x => x.Amount)
                .ToList();

            var topN = purchaseGroups.Take(5).ToList();
            var rest = purchaseGroups.Skip(5).Sum(x => x.Amount);
            var breakdownTotal = purchaseGroups.Sum(x => x.Amount);
            var breakdown = new List<BreakdownSlice>();
            for (int i = 0; i < topN.Count; i++)
            {
                breakdown.Add(new BreakdownSlice
                {
                    Name = topN[i].Name,
                    Amount = topN[i].Amount,
                    Percent = breakdownTotal > 0 ? (double)(topN[i].Amount / breakdownTotal) * 100 : 0,
                    Color = palette[i % palette.Length]
                });
            }
            if (rest > 0)
            {
                breakdown.Add(new BreakdownSlice
                {
                    Name = "Other",
                    Amount = rest,
                    Percent = breakdownTotal > 0 ? (double)(rest / breakdownTotal) * 100 : 0,
                    Color = palette[^1]
                });
            }

            var dashboard = new DashboardViewModel
            {
                UserId = user.Id,
                UserName = user.Name ?? "User",
                Balance = userViewModel.Balance,
                TotalSellers = _sellerService.GetAll().Count,
                TotalProducts = _productService.GetAll().Count,
                TotalStores = _storeService.GetAll().Count,
                TotalSpent = totalSpent,
                TotalEarned = totalEarned,
                TotalDeposited = totalDeposited,
                MonthSpent = monthSpent,
                MonthEarned = monthEarned,
                PrevMonthSpent = prevMonthSpent,
                PrevMonthEarned = prevMonthEarned,
                PrevMonthBalance = sparkBalance.Count >= 2 ? sparkBalance[^2] : 0m,
                Flow = flow,
                Breakdown = breakdown,
                SparkProfit = sparkProfit,
                SparkIncome = sparkIncome,
                SparkExpense = sparkExpense,
                SparkBalance = sparkBalance,
                LastTransactions = transactions.Take(5).ToList()
            };

            return View(dashboard);
        }
    }
}
