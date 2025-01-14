namespace Accounting_Software.Data.Entites
{
    public class TransactionHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string ProductName { get; set; } 
        public int Count { get; set; } 
        public decimal Price { get; set; }
        public decimal Total
        {
            get { return Price * Count; }
        }
        public DateTime SoldDate { get; set; } 
    }
}
