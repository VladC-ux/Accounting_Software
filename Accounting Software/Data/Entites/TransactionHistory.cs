using Accounting_Software.Enums;

namespace Accounting_Software.Data.Entites
{
    public class TransactionHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public Unit_of_mass unitOfmass { get; set; }
        public ushort Mass { get; set; }
        public int Count { get; set; }
        public decimal Total
        {
            get { return Price * Count; }
        }
        public string? StoreName { get; set; }
        public string typeofAction { get; set; } = null!;
        public DateTime SoldDate { get; set; } 
    }
}
