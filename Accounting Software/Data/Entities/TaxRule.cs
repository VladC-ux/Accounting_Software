using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting_Software.Data.Entities
{
    public class TaxRule
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        // Percentage applied to total income, e.g. 20.00 = 20%
        [Range(0, 100, ErrorMessage = "Rate must be between 0 and 100")]
        public decimal Rate { get; set; }

        public string? Description { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
