using Accounting_Software.Date.Entites;
using Accounting_Software.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting_Software.Data.Entites
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
            get { return Price * Count; }
        }
       
        [NotMapped]
        public double TotalPrice { get; set; }

        [ForeignKey("Seller")]
        public int SellerId { get; set; }
        public Seller Seller { get; set; }

        public ICollection<StoreProduct> StoreProducts { get; set; } = new List<StoreProduct>();

    }
}
