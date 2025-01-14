namespace Accounting_Software.Data.Entites
{
    public class User
    {
        public int Id { get; set; } 
        public decimal Balance { get; set; } = 0m;
        public ICollection<TransactionHistory> TransactionHistories { get; set; }
    }
}
