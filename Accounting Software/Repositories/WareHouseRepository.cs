using Accounting_Software.Data;
using Accounting_Software.Data.Entites;
using Accounting_Software.Date.Entites;
using Accounting_Software.Repository_Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace Accounting_Software.Repositories
{
    public class WareHouseRepository : IWareHouseRepositoryInterface
    {
        private readonly DBContextAccounting _context;

        public WareHouseRepository(DBContextAccounting context)
        {
            _context = context;
        }
        public void Add(WareHouse Warehouse)
        {
           _context.Add(Warehouse);
        }

        public void Delete(int id)
        {
            _context.Remove(id);
        }

        public List<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public double GetAveragePrice()
        {
            throw new NotImplementedException();
        }

        //public double GetAveragePrice()
        //{
        //    var averagePrice = _context.WareHouses.Average(wh => wh.Price);
        //    return averagePrice;
        //}

        public Product GetById(int id)
        {
            return _context.Products.FirstOrDefault(p=>p.Id == id);
           
        }

        public Product GetByName(string name)
        {
            return _context.Products.FirstOrDefault(p => p.Name == name);

        }
    }
}
