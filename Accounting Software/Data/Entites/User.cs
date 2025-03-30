namespace Accounting_Software.Data.Entites
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; } = 0m;
        public List<TransactionHistory> TransactionHistories { get; set; } = new List<TransactionHistory>();
    }
}
