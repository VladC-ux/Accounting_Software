using Accounting_Software.Data;
using Accounting_Software.Data.Entites;
using Accounting_Software.Date.Entites;
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

        public List<StoreProduct> GetStores()
        {
            return _context.StoreProducts.ToList();
        }
        public StoreProduct GetStoreProduct(int storeId, int productId)
        {
            var storeProduct = _context.Stores
                .Include(store => store.StoreProducts)
                .ThenInclude(sp => sp.Product)
                .Where(store => store.Id == storeId)
                .SelectMany(store => store.StoreProducts, (store, storeProduct) => new StoreProduct
                {
                    StoreId = store.Id,
                    ProductId = storeProduct.Product.Id,
                    StoreName = store.StoreName
                })
                .FirstOrDefault(sp => sp.ProductId == productId);

            return storeProduct;
        }
        public void Add(StoreProduct storeProduct)
        {
            _context.StoreProducts.Add(storeProduct);
            _context.SaveChanges();
        }

        public IEnumerable<StoreProduct> GetStoreProducts()
        {
            return _context.StoreProducts.Include(sp => sp.Store).Include(sp => sp.Product).ToList();
        }

        public List<StoreProduct> GetAll()
        {
            return _context.StoreProducts.ToList();
        }

        public List<StoreProduct> GetProductByStoreId(int storeId)
        {
            return _context.StoreProducts
                .Where(p => p.StoreId == storeId)
                .Include(p => p.Product) 
                .ThenInclude(p => p.Seller) 
                .ToList();
        }

        public List<string> GetSellerName()
        {
            return _context.Products
                .Select(p => p.Seller.Name)  
                .ToList();
        }


    }
}
