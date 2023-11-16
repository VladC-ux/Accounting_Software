using Accounting_Software.Date.Entites;
using Accounting_Software.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting_Software.Data.Entites
{
    public class Product
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public double Price { get; set; } 
        public Unit_of_mass Unitofmass { get; set; }

        [ForeignKey("Seller")]
        public int SellerId { get; set; }
        public Seller Seller { get; set; }


      

    }
}
