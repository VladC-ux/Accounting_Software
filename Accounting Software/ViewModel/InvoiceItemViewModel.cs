using Accounting_Software.Enums;
using System.ComponentModel.DataAnnotations;

namespace Accounting_Software.ViewModel
{
    public class InvoiceItemViewModel
    {
        public int Id { get; set; }
        public int? StoreProductId { get; set; }
        public int? ProductId { get; set; }

        [Required]
        public string ProductName { get; set; } = null!;

        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Count { get; set; }

        public ushort Mass { get; set; }
        public Unit_of_mass UnitOfMass { get; set; }

        public decimal Total => UnitOfMass == Unit_of_mass.Pcs || Mass <= 0
            ? Price * Count
            // Price is per base unit (kg / litre); grams and millilitres are 1/1000 of it.
            : Price * Count * (UnitOfMass == Unit_of_mass.Gram || UnitOfMass == Unit_of_mass.Ml ? Mass / 1000m : Mass);
    }
}
