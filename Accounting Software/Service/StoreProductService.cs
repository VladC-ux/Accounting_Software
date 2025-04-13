using Accounting_Software.Data.Entites;
using Accounting_Software.Enums;
using Accounting_Software.Repositories;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.UnitOfWorkk;
using Accounting_Software.ViewModel;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Accounting_Software.Service
{
    public class StoreProductService : IStoreProductService
    {
        private readonly IStoreProductRepository _storeProductRepository;
        private readonly IUnitofWork _uow;
        private readonly IProductRepository _productRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly ISellerRepository _sellerRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionHistoryRepository _transrepository;
        public StoreProductService(IStoreProductRepository storeproduct, IUnitofWork uow, IProductRepository productrepository, IStoreRepository storeRepositoryl, ISellerRepository sellerRepository, IUserRepository userRepository,ITransactionHistoryRepository transactionHistoryRepository)
        {
            _storeProductRepository = storeproduct;
            _uow = uow;
            _productRepository = productrepository;
            _storeRepository = storeRepositoryl;
            _sellerRepository = sellerRepository;
            _userRepository = userRepository;
            _transrepository = transactionHistoryRepository;
        }
        public void Add(StoreProductViewModel storeProduct)
        {
            StoreProduct st = new StoreProduct
            {
                Id = storeProduct.Id,
                ProductId = storeProduct.ProductId,
                StoreId = storeProduct.StoreId,
                Price = storeProduct.Price,
                AddDate = DateTime.Now,
                ProductName = storeProduct.ProductName,
                Mass = storeProduct.Mass,
                Description = storeProduct.Description,
                Count = storeProduct.Count,
                StoreName = storeProduct.StoreName,
                Unitofmass = storeProduct.unitOfmass,
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

            var storeProduct = new StoreProduct
            {
                ProductId = productId,
                StoreId = storeId
            };
            _storeProductRepository.Add(storeProduct);
        }

        public void Delete(StoreProductViewModel storeProduct)
        {
            var st = _storeProductRepository.GetById(storeProduct.Id);

            if (st == null)
            {
                return;
            }
            _storeProductRepository.Delete(st);
        }

        public StoreProductViewModel Update(StoreProductViewModel storeProduct)
        {
           
            StoreProduct st = new StoreProduct
            {
                Id = storeProduct.Id,
                StoreId = storeProduct.StoreId,
                ProductId = storeProduct.ProductId,        
                ProductName = storeProduct.ProductName,
                StoreName = storeProduct.StoreName,
                Price = storeProduct.Price,
                Description = storeProduct.Description,
                Unitofmass = storeProduct.unitOfmass,
                Mass = storeProduct.Mass,
                AddDate = storeProduct.AddDate,
                Count = storeProduct.Count,
            };
    
            _storeProductRepository.Update(st);
            _uow.SaveChanges();

            return new StoreProductViewModel
            {
                Id = st.Id,
                StoreId = storeProduct.StoreId,
                ProductId = storeProduct.ProductId,
                ProductName = st.ProductName,
                StoreName = st.StoreName,
                Price = st.Price,
                Description = st.Description,
                unitOfmass = st.Unitofmass,
                Mass = st.Mass,
                AddDate = st.AddDate,
                Count = st.Count,
            };
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

        public List<StoreProductViewModel> GetAll()
        {
            var data = _storeProductRepository.GetAll();
            List<StoreProductViewModel> storeproduct = data.Select(stProduct => new StoreProductViewModel
            {
                Id = stProduct.Id,
                StoreId = stProduct.StoreId,
                ProductId = stProduct.ProductId,
                ProductName = stProduct.ProductName,
                StoreName = stProduct.StoreName,
                Price = stProduct.Price,
                Description = stProduct.Description,
                unitOfmass = stProduct.Unitofmass,
                Mass = stProduct.Mass,
                AddDate = stProduct.AddDate,
                Count = stProduct.Count
            }).ToList();

            return storeproduct;
        }
        public List<StoreProductViewModel> GetProductByStoreId(int? storeid)
        {
            if (storeid.HasValue)
            {
                var data = _storeProductRepository.GetProductByStoreId(storeid.Value);

                List<StoreProductViewModel> storeproduct = data.Select(st =>
                {
                    return new StoreProductViewModel
                    {
                        Id = st.Id,
                        StoreId = st.StoreId,
                        ProductId = st.ProductId,
                        ProductName = st.ProductName,
                        StoreName = st.StoreName,
                        Price = st.Price,
                        Description = st.Description,
                        unitOfmass = st.Unitofmass,
                        Mass = st.Mass,
                        AddDate = st.AddDate,
                        Count = st.Count,
                        SellerName = st.Product.Seller.Name
                    };
                }).ToList();

                return storeproduct;
            }
            return new List<StoreProductViewModel>();
        }


        public StoreProductViewModel GetById(int storeId)
        {
            var st = _storeProductRepository.GetById(storeId);  
            return new StoreProductViewModel
            {
                Id = st.Id,
                StoreId = st.StoreId,
                ProductId = st.ProductId,
                ProductName = st.ProductName,
                StoreName = st.StoreName,
                Price = st.Price,
                Description = st.Description,
                unitOfmass = st.Unitofmass,
                Mass = st.Mass,
                AddDate = st.AddDate,
                Count = st.Count,
            };
        }

        public void GetBalanceSale(int storeid,int userid)
        {
            var data = _storeProductRepository.GetById(storeid);
            var user = _userRepository.GetUserById(userid);
            user.Balance += data.Price;
            _storeProductRepository.Delete(data);

            TransactionHistory transactionHistory = new TransactionHistory()
            {
                ProductName = data.ProductName,
                Price = data.Price,
                Description = data.Description,
                Mass = data.Mass,
                UserId = userid,
                unitOfmass = data.Unitofmass,
                Count = data.Count,
                SoldDate = DateTime.Now,
                typeofAction = "Sale",
                StoreName = data.StoreName
            };
            _transrepository.Add(transactionHistory);
            _uow.SaveChanges();
        }
    }
}