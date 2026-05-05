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

        // This month
        public decimal MonthSpent { get; set; }
        public decimal MonthEarned { get; set; }
        public decimal TotalDeposited { get; set; }

        public List<TransactionHistoryViewModel> LastTransactions { get; set; } = new();
    }
}
