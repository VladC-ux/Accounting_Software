using Accounting_Software.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting_Software.Data.Entities
{
    public class InvoiceItem
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }

        public int? StoreProductId { get; set; }
        public int? ProductId { get; set; }

        public string ProductName { get; set; } = null!;
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Count { get; set; }

        public ushort Mass { get; set; }
        public Unit_of_mass UnitOfMass { get; set; }

        [NotMapped]
        public decimal Total => UnitOfMass != Unit_of_mass.Pcs && Mass > 0
            ? Price * Count * Mass
            : Price * Count;
    }
}
