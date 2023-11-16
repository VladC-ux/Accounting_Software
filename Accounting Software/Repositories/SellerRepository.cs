using Accounting_Software.Data;
using Accounting_Software.Data.Entites;
using Accounting_Software.Repository_Interfaces;

namespace Accounting_Software.Repositories
{
    public class SellerRepository : ISellerRepositoryInterface
    {
        private readonly DBContextAccounting _context;

        public SellerRepository(DBContextAccounting context)
        {
            _context = context;
        }

        public void Add(Seller Seller)
        {
            _context.Sellers.Add(Seller);
            _context.SaveChanges();
        }

        public void Delete(Seller Seller)
        {
            _context.Sellers.Remove(Seller);
            _context.SaveChanges();
        }

        public List<Seller> GetAll()
        {
            return _context.Sellers.ToList();
        }

        public Seller GetById(int id)
        {
            return _context.Sellers.FirstOrDefault(s => s.Id ==  id);
        }

        public Seller Update(Seller Seller)
        {
            var entity = _context.Sellers.FirstOrDefault(s =>s.Id == Seller.Id);
            if(entity!=null)
            {
                entity.Name = Seller.Name;

                _context.Sellers.Update(entity);
                _context.SaveChanges();


                return entity;

            }
            else
            {
                throw new NotImplementedException("The Seller is not found");
            }
          
        }
    }
}
