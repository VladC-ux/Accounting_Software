using Accounting_Software.Data.Entities;
using Accounting_Software.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting_Software.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public ushort Mass { get; set; }
        public Unit_of_mass Unitofmass { get; set; }
        public int Count { get; set; }
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
       
        [NotMapped]
        public double TotalPrice { get; set; }

        [ForeignKey("Seller")]
        public int SellerId { get; set; }
        public Seller Seller { get; set; }

        public ICollection<StoreProduct> StoreProducts { get; set; } = new List<StoreProduct>();

    }
}
