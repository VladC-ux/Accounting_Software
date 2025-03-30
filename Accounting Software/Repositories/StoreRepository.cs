using Accounting_Software.Data.Entites;
using Accounting_Software.Data;
using Accounting_Software.Repository_Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;

namespace Accounting_Software.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly DBContextAccounting _context;

        public StoreRepository(DBContextAccounting context)
        {
            _context = context;
        }

        public Store GetById(int id)
        {
            return _context.Stores.FirstOrDefault(p => p.Id == id);
        }
        
        public List<Store> GetAll()
        {
            return _context.Stores.ToList();
        }

        public void Add(Store store)
        {
            _context.Stores.Add(store);          
        }

        public void Update(Store store)
        {
            _context.Stores.Update(store);
            
        }

        public void Delete(int id)
        {
            var store = GetById(id);
            if (store != null)
            {
                _context.Stores.Remove(store);
              
            }
        }

           
    }
}
