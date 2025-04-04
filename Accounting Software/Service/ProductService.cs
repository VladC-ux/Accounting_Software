using Accounting_Software.Data.Entites;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Accounting_Software.UnitOfWorkk;
using Microsoft.CodeAnalysis;
using Accounting_Software.Date.Entites;

namespace Accounting_Software.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IStoreRepository _store;
        private readonly IUnitofWork _uow;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionHistoryRepository _transRepository;

        public ProductService(IProductRepository poroductrepository, IUnitofWork uow, IStoreRepository store,IUserRepository userRepository,ITransactionHistoryRepository transactionHistoryRepository)
        {
            _productRepository = poroductrepository;
            _uow = uow;        
            _store = store;
            _userRepository = userRepository;
            _transRepository = transactionHistoryRepository;
        }

        public void Add(ProductViewModel model) 
        {

            Product product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                Mass = model.Mass,
                Unitofmass = model.unitOfmass,
                SellerId = model.SellerId,
                Count = model.Count,
               
            };
            var data = _userRepository.GetUserById(4);
            data.Balance -= model.Total;
            _productRepository.Add(product);
            _uow.SaveChanges();


            TransactionHistory transactionhistory = new TransactionHistory()
            {
                ProductName = model.Name,
                Price = model.Price,
                Description = model.Description,
                Mass = model.Mass,
                unitOfmass = model.unitOfmass,
                Count = model.Count,
            };
            _transRepository.Add(transactionhistory);
            

        }      
        public void Delete(ProductViewModel model)
        {
            _productRepository.Delete(model.Id);
            _uow.SaveChanges();
        }
        public List<ProductViewModel> GetAll()
        {
            var data = _productRepository.GetAll();
            List<ProductViewModel> productViewModels = data.Select(product => new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                unitOfmass = product.Unitofmass,
                Mass = product.Mass

            }).ToList();
            return productViewModels;
        }
        public ProductViewModel GetById(int ProductId)
        {
            var product = _productRepository.GetById(ProductId);
            return new ProductViewModel
            {

                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                unitOfmass = product.Unitofmass,
                Mass = product.Mass,
                SellerId = product.SellerId,
                
            };
        }
        public List<ProductViewModel> GetProductsBySellerId(int? sellerId)
        {
            if (sellerId.HasValue)
            {
                var data = _productRepository.GetProductsBySellerId(sellerId.Value);
                List<ProductViewModel> productViewModels = data.Select(product => new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    unitOfmass = product.Unitofmass,
                    Mass = product.Mass,
                    Count= product.Count,
                    SellerId = product.SellerId

                }).ToList();
                return productViewModels;
            }
            return new List<ProductViewModel>();
        }
       

        public void Update(ProductViewModel Product)
        {
            Product product = new Product
            {
                Id = Product.Id,
                Name = Product.Name,
                Price = Product.Price,
                Mass = Product.Mass,
                Unitofmass = Product.unitOfmass,
                Description = Product.Description,
            };
            var data = _productRepository.Update(product);
            _uow.SaveChanges();
        }

      
    }
}
