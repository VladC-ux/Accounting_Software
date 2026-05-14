namespace Accounting_Software.ViewModel
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public string Number { get; set; } = null!;
        public DateTime IssuedAt { get; set; }

        public int UserId { get; set; }
        public string? UserName { get; set; }

        public int? StoreId { get; set; }
        public string? StoreName { get; set; }

        public string? BuyerName { get; set; }
        public string? BuyerEmail { get; set; }
        public string? BuyerPhone { get; set; }

        public decimal TaxRate { get; set; }

        public List<InvoiceItemViewModel> Items { get; set; } = new();

        public decimal Subtotal => Items?.Sum(i => i.Total) ?? 0m;
        public decimal TaxAmount => Math.Round(Subtotal * TaxRate / 100m, 2);
        public decimal Total => Subtotal + TaxAmount;
    }
}
