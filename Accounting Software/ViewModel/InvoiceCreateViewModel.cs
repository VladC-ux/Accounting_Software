using System.ComponentModel.DataAnnotations;

namespace Accounting_Software.ViewModel
{
    public class InvoiceCreateViewModel
    {
        [Required]
        public int StoreId { get; set; }

        public string? StoreName { get; set; }

        public string? BuyerName { get; set; }

        [EmailAddress]
        public string? BuyerEmail { get; set; }

        public string? BuyerPhone { get; set; }

        [Range(0, 100, ErrorMessage = "Tax rate must be between 0 and 100")]
        public decimal TaxRate { get; set; }

        public List<InvoiceCreateItemViewModel> Items { get; set; } = new();
    }

    public class InvoiceCreateItemViewModel
    {
        [Required]
        public int StoreProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Count { get; set; }
    }
}
