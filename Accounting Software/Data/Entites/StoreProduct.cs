using Accounting_Software.Data.Entites;
using Accounting_Software.Enums;
using System.ComponentModel.DataAnnotations;

public class StoreProduct
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public Store Store { get; set; }
    public int ProductId { get; set; }
    public int SellerId { get; set; }
    public string ProductName { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
  
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
    public decimal Price { get; set; }
    public DateTime AddDate { get; set; }

    public decimal Total
    {
        get { return Price * Count; }
    }

    public string? Description { get; set; }
    public ushort Mass { get; set; }
    public Unit_of_mass Unitofmass { get; set; }
    public int Count { get; set; }
}
