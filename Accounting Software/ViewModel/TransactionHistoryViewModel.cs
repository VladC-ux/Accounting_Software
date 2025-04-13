using Accounting_Software.Data.Entites;
using Accounting_Software.Enums;

namespace Accounting_Software.ViewModel
{
    public class TransactionHistoryViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public Unit_of_mass unitOfmass { get; set; }
        public ushort Mass { get; set; }
        public int Count { get; set; }
        public decimal Total
        {
            get { return Price * Count; }
        }
        public string ProductName { get; set; } = null!;
        public string typeofAction { get; set; } = null!;

        public string? StoreName { get; set; }
        public DateTime SoldDate { get; set; }
    }
}
