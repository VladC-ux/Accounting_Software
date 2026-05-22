using Accounting_Software.Data.Entities;
using Accounting_Software.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting_Software.ViewModel
{
    public class StoreProductViewModel
    {
        public int Id { get; set; }

        [ForeignKey("Product")]
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
            get
            {
                if (unitOfmass == Unit_of_mass.Pcs || Mass <= 0)
                    return Price * Count;

                // Price is per base unit (kg / litre); grams and millilitres are 1/1000 of it.
                decimal quantity = unitOfmass == Unit_of_mass.Gram || unitOfmass == Unit_of_mass.Ml
                    ? Mass / 1000m
                    : Mass;
                return Price * Count * quantity;
            }
        }

        public string SellerName { get; set; }
      
    }
}
