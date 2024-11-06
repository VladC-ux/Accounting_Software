using System.Collections.Generic;

namespace Accounting_Software.Data.Entites
{
    public class Store
    {
        public int Id { get; set; }
        public string StoreName { get; set; } 
        public ICollection<StoreProduct> StoreProducts { get; set; } = new List<StoreProduct>();
    }
}