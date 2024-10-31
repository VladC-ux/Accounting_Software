using Accounting_Software.Data.Entites;
using System.ComponentModel.DataAnnotations;

public class StoreProduct
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public Store Store { get; set; }
    public int ProductId { get; set; }
    public Product ProductName { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
    public decimal Price { get; set; }
    public DateTime AddDate { get; set; }
}
