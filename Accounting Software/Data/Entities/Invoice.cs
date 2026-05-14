using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting_Software.Data.Entities
{
    public class Invoice
    {
        public int Id { get; set; }

        public string Number { get; set; } = null!;

        public DateTime IssuedAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }
        public User? User { get; set; }

        public int? StoreId { get; set; }
        public Store? Store { get; set; }
        public string? StoreName { get; set; }

        public string? BuyerName { get; set; }
        public string? BuyerEmail { get; set; }
        public string? BuyerPhone { get; set; }

        public decimal TaxRate { get; set; }

        public List<InvoiceItem> Items { get; set; } = new();

        [NotMapped]
        public decimal Subtotal => Items?.Sum(i => i.Total) ?? 0m;

        [NotMapped]
        public decimal TaxAmount => Math.Round(Subtotal * TaxRate / 100m, 2);

        [NotMapped]
        public decimal Total => Subtotal + TaxAmount;
    }
}
