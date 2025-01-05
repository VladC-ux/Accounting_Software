using Accounting_Software.Data.Entites;
using Accounting_Software.Enums;

namespace Accounting_Software.ViewModel
{
    public class StoreProductViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; } 
        public string ProductName { get; set; }
        public DateTime AddDate { get; set; }
        public decimal Price { get; set; }
        public ushort Mass { get; set; }
        public int Count { get; set; }
        public Unit_of_mass unitOfmass { get; set; }
        public string? Description { get; set; }

        public decimal Total
        {
            get { return Price * Count; }
        }

        public string SellerName { get; set; }
      
    }
}
