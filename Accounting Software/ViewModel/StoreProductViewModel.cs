using Accounting_Software.Data.Entites;

namespace Accounting_Software.ViewModel
{
    public class StoreProductViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product ProductName { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public int Quantity { get; set; }
        public DateTime AddDate { get; set; }
        public decimal Price { get; set; }
    }
}
