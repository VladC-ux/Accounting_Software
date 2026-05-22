using System.ComponentModel.DataAnnotations;

namespace Accounting_Software.ViewModel
{
    public class TaxRuleViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a tax name")]
        public string Name { get; set; } = null!;

        [Range(0.01, 100, ErrorMessage = "Rate must be between 0 and 100")]
        public decimal Rate { get; set; }

        public string? Description { get; set; }

        public int UserId { get; set; }

        // Amount owed for this rule = TaxBase * Rate / 100 (filled by the service)
        public decimal Amount { get; set; }
    }
}
