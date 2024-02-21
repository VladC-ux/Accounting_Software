using Accounting_Software.Data;
using Accounting_Software.Data.Entites;
using Accounting_Software.Date.Entites;
using Accounting_Software.Repository_Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Accounting_Software.Repositories
{
    public class ProductRepository : IProductRepositoryInterface
    {
        private readonly DBContextAccounting _context;

        public ProductRepository(DBContextAccounting context)
        {
            _context = context;
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            

        }
        public void MoveToWarehouse(int productId, WareHouse warehouse)
        {
            var querry = _context.Products.Find(productId);
                     
        }



        public void Delete(int product)
        {
            var querry = _context.Products.Find(product);
            if (querry != null)
            {
                _context.Products.Remove(querry);
            }


        }

        public List<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public Product GetById(int ProductId)
        {
           
            return _context.Products.FirstOrDefault(p => p.Id == ProductId);

        }

        public List<Product> GetProductsBySellerId(int sellerId)
        {
            return _context.Products.Where(p => p.SellerId == sellerId).ToList();
        }

        public Product Update(Product product)
        {
            var entity = _context.Products.FirstOrDefault(p => p.Id == product.Id);

            entity.Name = product.Name;
            entity.Price = product.Price;
            entity.Mass = product.Mass;
            entity.Description = product.Description;
            entity.Unitofmass = product.Unitofmass;
            _context.Products.Update(entity);
            _context.SaveChanges();
            return entity;


        }

      
        public void AddToWareHouse(WareHouse product)
        {
            _context.Add(product);
        }

        public Product GetWareHouseViewModelById(int id)
        {
            throw new NotImplementedException();
        }
    }
}