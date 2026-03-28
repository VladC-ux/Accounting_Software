namespace Accounting_Software.ViewModel
{
    public class TransactionFilterViewModel
    {
        public int UserId { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? ActionType { get; set; } // "Add", "Sale", or null = all

        public List<TransactionHistoryViewModel> Transactions { get; set; } = new();

        public decimal TotalSpent => Transactions
            .Where(t => t.typeofAction == "Add")
            .Sum(t => t.Total);

        public decimal TotalEarned => Transactions
            .Where(t => t.typeofAction == "Sale")
            .Sum(t => t.Total);
    }
}
