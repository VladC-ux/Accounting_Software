using Accounting_Software.Data;
using Accounting_Software.Date.Entites;
using Accounting_Software.Repository_Interfaces;
using Microsoft.Identity.Client;

namespace Accounting_Software.Repositories
{
    public class SellerRepository : ISellerRepositoryInterface
    {
        private readonly DBContextAccounting _context;

        public SellerRepository(DBContextAccounting context)
        {
            _context = context;
        }

        public void Add(Seller user)
        {
            _context.Sellers.Add(user);
        }

        public void Delete(int id)
        {
            var querry = _context.Sellers.Find(id);

            if (querry != null)
            {
                _context.Sellers.Remove(querry);
                
            }
        }
        public List<Seller> GetAll()
        {
            var entity = _context.Sellers.ToList();
            return entity;
        }

        public Seller GetById(int id)
        {
            return _context.Sellers.FirstOrDefault(u => u.Id == id);
        }

        public Seller Update(Seller user)
        {
            var entity = _context.Sellers.FirstOrDefault(u => u.Id == user.Id);
           
            
                entity.Name = user.Name;
                _context.Sellers.Update(entity);
             

                return entity;
            
         
        }
    }
}
