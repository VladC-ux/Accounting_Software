using Accounting_Software.Data.Entites;
using Accounting_Software.Repositories;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.UnitOfWorkk;
using Accounting_Software.ViewModel;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Accounting_Software.Service
{
    public class StoreProductService : IStoreProductService
    {
        private readonly IStoreProductRepository _sp;
        private readonly IUnitofWork _uow;
        private readonly IProductRepository _productRepository;

        public StoreProductService(IStoreProductRepository storeproduct, IUnitofWork uow, IProductRepository productrepository)
        {
            _sp = storeproduct;
            _uow = uow;
            _productRepository = productrepository;
        }
        public void Add(StoreProductViewModel storeProduct)
        {
            StoreProduct st = new StoreProduct
            {
                ProductId = storeProduct.ProductId,
                StoreId = storeProduct.StoreId,             
                Price = storeProduct.Price,
                AddDate = DateTime.Now,
                ProductName = storeProduct.ProductName,
            };

            _sp.Add(st);
            _uow.SaveChanges();
        }

        public Task AddAsync(StoreProduct storeProduct)
        {
            throw new NotImplementedException();
        }

        public async Task AddProductToStoreAsync(int productId)
        {         
            var product = await _productRepository.GetProductByIdAsync(productId);

            if (product == null)
            {
                throw new Exception("Product not found");
            }

            StoreProduct storeProduct = new StoreProduct
            {
                Id = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Mass = product.Mass,
                Unitofmass = product.Unitofmass,
                Description = product.Description,
                SellerId = product.SellerId    
                
            }; 
            
            await _sp.AddAsync(storeProduct);
            await _uow.SaveChangesAsnyc();
        }

        public void Delete(int storeId, int productId)
        {
           
            var storeProduct = _sp.GetById(storeId, productId);
            if (storeProduct == null)
            {
                throw new KeyNotFoundException("StoreProduct not found.");
            }

            _sp.Delete(storeId, productId);            
            _uow.SaveChanges();
        }      
        public StoreProductViewModel GetById(int storeId, int productId)
        {       
            var storeProduct = _sp.GetById(storeId, productId);

            if (storeProduct == null)
            {
                throw new KeyNotFoundException("StoreProduct not found.");
            }          
            return new StoreProductViewModel
            {
                ProductId = storeProduct.ProductId,
                StoreId = storeProduct.StoreId,              
                Price = storeProduct.Price,
                AddDate = storeProduct.AddDate,
                ProductName = storeProduct.ProductName 
            };
        }

        public IEnumerable<StoreProductViewModel> GetByStoreId(int storeId)
        {
            var storeProducts = _sp.GetByStoreId(storeId);
            if (storeProducts == null)
            {
                throw new KeyNotFoundException("Store not found.");
            }
            return storeProducts.Select(sp => new StoreProductViewModel
            {
                ProductId = sp.ProductId,
                StoreId = sp.StoreId,              
                Price = sp.Price,
                AddDate = sp.AddDate,
                ProductName = sp.ProductName

            }).ToList();
        }

        public void Update(StoreProductViewModel storeProduct)
        {
            StoreProduct sp = new StoreProduct
            {
                Id = storeProduct.Id,
                ProductId = storeProduct.ProductId,
                StoreId = storeProduct.StoreId,
                Price = storeProduct.Price,
                AddDate = storeProduct.AddDate,
                ProductName = storeProduct.ProductName
            };

            _sp.Update(sp);
            _uow.SaveChanges();
        }

    }
}
