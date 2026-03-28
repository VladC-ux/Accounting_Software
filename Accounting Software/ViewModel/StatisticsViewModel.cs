namespace Accounting_Software.ViewModel
{
    public class StatisticsViewModel
    {
        public int UserId { get; set; }

        // Totals
        public decimal TotalSpent { get; set; }
        public decimal TotalEarned { get; set; }
        public int TotalPurchases { get; set; }
        public int TotalSales { get; set; }
        public decimal NetProfit => TotalEarned - TotalSpent;

        // Monthly chart (last 6 months)
        public List<string> MonthLabels { get; set; } = new();
        public List<decimal> MonthlySpent { get; set; } = new();
        public List<decimal> MonthlyEarned { get; set; } = new();

        // Top 5 products by sales revenue
        public List<ChartItem> TopSoldProducts { get; set; } = new();

        // Top 5 products by purchase spend
        public List<ChartItem> TopBoughtProducts { get; set; } = new();

        // Revenue by store
        public List<ChartItem> RevenueByStore { get; set; } = new();
    }

    public class ChartItem
    {
        public string Label { get; set; }
        public decimal Value { get; set; }
    }
}
