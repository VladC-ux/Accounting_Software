using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionHistoryService _transactionHistoryService;

        public StatisticsController(IUserRepository userRepository, ITransactionHistoryService transactionHistoryService)
        {
            _userRepository = userRepository;
            _transactionHistoryService = transactionHistoryService;
        }

        public IActionResult Index()
        {
            if (_userRepository.UserCount() == 0)
                return RedirectToAction("AddUser", "User");

            var user = _userRepository.GetAll().First();
            var transactions = _transactionHistoryService.GetHistoryByUserId(user.Id);

            var purchases = transactions.Where(t => t.typeofAction == "Add").ToList();
            var sales     = transactions.Where(t => t.typeofAction == "Sale").ToList();

            // --- Monthly chart: last 6 months ---
            var monthLabels   = new List<string>();
            var monthlySpent  = new List<decimal>();
            var monthlyEarned = new List<decimal>();

            for (int i = 5; i >= 0; i--)
            {
                var month = DateTime.Now.AddMonths(-i);
                monthLabels.Add(month.ToString("MMM yyyy"));

                monthlySpent.Add(purchases
                    .Where(t => t.SoldDate.Year == month.Year && t.SoldDate.Month == month.Month)
                    .Sum(t => t.Total));

                monthlyEarned.Add(sales
                    .Where(t => t.SoldDate.Year == month.Year && t.SoldDate.Month == month.Month)
                    .Sum(t => t.Total));
            }

            // --- Top 5 products by sales ---
            var topSold = sales
                .GroupBy(t => t.ProductName)
                .Select(g => new ChartItem { Label = g.Key, Value = g.Sum(t => t.Total) })
                .OrderByDescending(x => x.Value)
                .Take(5)
                .ToList();

            // --- Top 5 products by purchases ---
            var topBought = purchases
                .GroupBy(t => t.ProductName)
                .Select(g => new ChartItem { Label = g.Key, Value = g.Sum(t => t.Total) })
                .OrderByDescending(x => x.Value)
                .Take(5)
                .ToList();

            // --- Revenue by store ---
            var byStore = sales
                .Where(t => !string.IsNullOrEmpty(t.StoreName))
                .GroupBy(t => t.StoreName)
                .Select(g => new ChartItem { Label = g.Key, Value = g.Sum(t => t.Total) })
                .OrderByDescending(x => x.Value)
                .ToList();

            var model = new StatisticsViewModel
            {
                UserId        = user.Id,
                TotalSpent    = purchases.Sum(t => t.Total),
                TotalEarned   = sales.Sum(t => t.Total),
                TotalPurchases = purchases.Count,
                TotalSales    = sales.Count,
                MonthLabels   = monthLabels,
                MonthlySpent  = monthlySpent,
                MonthlyEarned = monthlyEarned,
                TopSoldProducts   = topSold,
                TopBoughtProducts = topBought,
                RevenueByStore    = byStore,
            };

            return View(model);
        }
    }
}
