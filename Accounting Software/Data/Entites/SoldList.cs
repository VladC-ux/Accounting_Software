using Accounting_Software.Date.Entites;
using Accounting_Software.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting_Software.Data.Entites
{
    public class SoldList
    {
        public int Id { get; set; }

        public int Price { get; set; }

        public int Count { get; set; }

        public Unit_of_mass Unitofmass { get; set; }


        [ForeignKey("WareHouse")]
        public int WareHouseId { get; set; }    
        public WareHouse WareHouse { get; set; }


    }
}
