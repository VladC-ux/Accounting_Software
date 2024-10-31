using Accounting_Software.Data.Entites;
using Accounting_Software.Repositories;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.UnitOfWorkk;
using Accounting_Software.ViewModel;
using Microsoft.CodeAnalysis;

namespace Accounting_Software.Service
{
    public class StoreProductService : IStoreProductService
    {
        private readonly IStoreProductRepository _sp;
        private readonly IUnitofWork _uow;

        public StoreProductService(IStoreProductRepository storeproduct, IUnitofWork uow)
        {
            _sp = storeproduct;
            _uow = uow;          
        }
        public void Add(StoreProductViewModel storeProduct)
        {
            StoreProduct st = new StoreProduct
            {
                ProductId = storeProduct.ProductId,
                StoreId = storeProduct.StoreId,
                Quantity = storeProduct.Quantity,
                Price = storeProduct.Price,
                AddDate = DateTime.Now,
                ProductName = storeProduct.ProductName,
            };

            _sp.Add(st);
            _uow.SaveChanges();
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
                Quantity = storeProduct.Quantity,
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
                Quantity = sp.Quantity,
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
                Quantity = storeProduct.Quantity,
                Price = storeProduct.Price,
                AddDate = storeProduct.AddDate,
                ProductName = storeProduct.ProductName
            };

            _sp.Update(sp);
            _uow.SaveChanges();
        }

    }
}
