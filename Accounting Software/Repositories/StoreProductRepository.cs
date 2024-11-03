using Accounting_Software.Data;
using Accounting_Software.Data.Entites;
using Accounting_Software.Repository_Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Accounting_Software.Repositories
{
    public class StoreProductRepository : IStoreProductRepository
    {
        public readonly DBContextAccounting _context;

        public StoreProductRepository(DBContextAccounting context)
        {
            _context = context;
        }

        public void Add(StoreProduct storeProduct)
        {
            _context.StoreProducts.Add(storeProduct);
        }

        public async Task AddAsync(StoreProduct storeProduct)
        {
            await _context.StoreProducts.AddAsync(storeProduct);
        }

        public void Delete(int storeId, int productId)
        {
            var storeProduct = GetById(storeId, productId);
            if (storeProduct != null)
            {
                _context.StoreProducts.Remove(storeProduct);
            }
        }

        public StoreProduct GetById(int storeId, int productId)
        {
            return _context.StoreProducts
            .FirstOrDefault(sp => sp.StoreId == storeId && sp.ProductId == productId);
        }

       
        public IEnumerable<StoreProduct> GetByStoreId(int storeId)
        {
            return _context.StoreProducts
            .Where(sp => sp.StoreId == storeId).ToList();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public void Update(StoreProduct storeProduct)
        {
            _context.StoreProducts.Update(storeProduct);
        }

    }
}
