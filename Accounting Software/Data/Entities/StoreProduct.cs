using Accounting_Software.Data.Entities;
using Accounting_Software.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting_Software.Data.Entities
{
    public class StoreProduct
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }

        [ForeignKey("Id")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string ProductName { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }
        public DateTime AddDate { get; set; }
        public decimal Total
        {
            get
            {
                if (Unitofmass == Unit_of_mass.Pcs || Mass <= 0)
                    return Price * Count;

                // Price is per base unit (kg / litre); grams and millilitres are 1/1000 of it.
                decimal quantity = Unitofmass == Unit_of_mass.Gram || Unitofmass == Unit_of_mass.Ml
                    ? Mass / 1000m
                    : Mass;
                return Price * Count * quantity;
            }
        }
        public string? Description { get; set; }
        public ushort Mass { get; set; }
        public Unit_of_mass Unitofmass { get; set; }
        public int Count { get; set; }
    }
}
