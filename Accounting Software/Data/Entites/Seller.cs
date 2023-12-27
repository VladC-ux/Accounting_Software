using Accounting_Software.Data.Entites;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting_Software.Date.Entites
{
    public class Seller
    {
        public int Id { get; set; }
        public string Name { get; set; }     
        public ICollection<Product> Products { get; set; }
    }

}
