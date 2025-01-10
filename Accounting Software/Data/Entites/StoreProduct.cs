using Accounting_Software.Data.Entites;
using Accounting_Software.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting_Software.Data.Entites
{
    public class StoreProduct
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }

        [ForeignKey("Id")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string ProductName { get; set; }


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


}
