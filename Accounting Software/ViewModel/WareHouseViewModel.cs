using Accounting_Software.Enums;

namespace Accounting_Software.ViewModel
{
    public class WareHouseViewModel
    {
        public int Id { get; set; }
        public DateTime DateBuy { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public double Balance { get; set; }
        public Unit_of_mass Unitofmass { get; set; }

    }
}
