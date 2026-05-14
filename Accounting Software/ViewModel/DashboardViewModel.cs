namespace Accounting_Software.ViewModel
{
    public class DashboardViewModel
    {
        public decimal Balance { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }

        public int TotalSellers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalStores { get; set; }

        public decimal TotalSpent { get; set; }
        public decimal TotalEarned { get; set; }

        public decimal MonthSpent { get; set; }
        public decimal MonthEarned { get; set; }
        public decimal TotalDeposited { get; set; }

        public decimal PrevMonthSpent { get; set; }
        public decimal PrevMonthEarned { get; set; }
        public decimal PrevMonthBalance { get; set; }

        public List<MonthlyFlowPoint> Flow { get; set; } = new();
        public List<BreakdownSlice> Breakdown { get; set; } = new();

        public List<decimal> SparkProfit { get; set; } = new();
        public List<decimal> SparkIncome { get; set; } = new();
        public List<decimal> SparkExpense { get; set; } = new();
        public List<decimal> SparkBalance { get; set; } = new();

        public List<TransactionHistoryViewModel> LastTransactions { get; set; } = new();
    }

    public class MonthlyFlowPoint
    {
        public string Label { get; set; } = "";
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
    }

    public class BreakdownSlice
    {
        public string Name { get; set; } = "";
        public decimal Amount { get; set; }
        public double Percent { get; set; }
        public string Color { get; set; } = "";
    }
}
