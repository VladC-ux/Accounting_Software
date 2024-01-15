using Accounting_Software.Enums;
using Accounting_Software.Data.Entites;
using Accounting_Software.Date.Entites;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting_Software.ViewModel
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string? Description { get; set; }

        public Unit_of_mass unitOfmass { get; set; }
        public ushort Mass { get; set; }

        public int SellerId { get; set; }

       public int? WareHosueId { get; set;}
       
    }
}
