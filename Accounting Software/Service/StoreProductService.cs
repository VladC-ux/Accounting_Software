using Accounting_Software.Data.Entites;
using Accounting_Software.Repositories;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.UnitOfWorkk;
using Accounting_Software.ViewModel;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Accounting_Software.Service
{
    public class StoreProductService : IStoreProductService
    {
        private readonly IStoreProductRepository _storeProductRepository;
        private readonly IUnitofWork _uow;
        private readonly IProductRepository _productRepository;
        private readonly IStoreRepository _storeRepository;

        public StoreProductService(IStoreProductRepository storeproduct, IUnitofWork uow, IProductRepository productrepository, IStoreRepository storeRepositoryl)
        {
            _storeProductRepository = storeproduct;
            _uow = uow;
            _productRepository = productrepository;
            _storeRepository = storeRepositoryl;
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

            _storeProductRepository.Add(st);
            _uow.SaveChanges();
        }

        public Task AddAsync(StoreProduct storeProduct)
        {
            throw new NotImplementedException();
        }

        public void AddProductToStore(int productId, int storeId)
        {
           
            var product = _productRepository.GetById(productId);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }


            var store = _storeRepository.GetById(storeId);
            if (store == null)
            {
                throw new Exception("Store not found.");
            }


            var storeProductExists = _storeProductRepository.Exists(storeId, productId);
            if (storeProductExists)
            {
                throw new Exception("Product is already added to this store.");
            }

            var storeProduct = new StoreProduct
            {
                ProductId = productId,
                StoreId = storeId
            };

            
            _storeProductRepository.Add(storeProduct);
        }

        public void Delete(int storeId, int productId)
        {
           
            var storeProduct = _storeProductRepository.GetById(storeId, productId);
            if (storeProduct == null)
            {
                throw new KeyNotFoundException("StoreProduct not found.");
            }

            _storeProductRepository.Delete(storeId, productId);            
            _uow.SaveChanges();
        }      
        public StoreProductViewModel GetById(int storeId, int productId)
        {       
            var storeProduct = _storeProductRepository.GetById(storeId, productId);

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
            var storeProducts = _storeProductRepository.GetByStoreId(storeId);
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
                
                ProductId = storeProduct.ProductId,
                StoreId = storeProduct.StoreId,
                Price = storeProduct.Price,
                AddDate = storeProduct.AddDate,
                ProductName = storeProduct.ProductName
            };

            _storeProductRepository.Update(sp);
            _uow.SaveChanges();
        }
        public List<StoreProductViewModel> GetStores()
        {
            var data = _storeProductRepository.GetAll();
            List<StoreProductViewModel> storeProducts = data.Select(storeproduct => new StoreProductViewModel
            {

              StoreId = storeproduct.StoreId,
              ProductId = storeproduct.ProductId,
              StoreName = storeproduct.StoreName,

            }).ToList();

            return storeProducts;
        }

        public StoreProduct GetStoreProduct(int storeId, int productId)
        {
            return _storeProductRepository.GetStoreProduct(storeId, productId);
        }

       

    }
}
