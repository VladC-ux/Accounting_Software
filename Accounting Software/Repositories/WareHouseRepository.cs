using Accounting_Software.Data;
using Accounting_Software.Data.Entites;
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
        public void Add(Product Product)
        {
           _context.Add(Product);
        }

        public List<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public double GetAveragePrice()
        {
            var averagePrice = _context.WareHouses.Average(wh => wh.Price);
            return averagePrice;
        }

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
