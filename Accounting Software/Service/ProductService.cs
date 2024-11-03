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
        private readonly IWareHouseRepository _Wrepo;
        private readonly IProductRepository _productRepository;
        private readonly IStoreRepository _store;
        private readonly IUnitofWork _uow;


        public ProductService(IProductRepository poroductrepository, IUnitofWork uow, IWareHouseRepository wrepo , IStoreRepository store)
        {
            _productRepository = poroductrepository;
            _uow = uow;
            _Wrepo = wrepo;
            _store = store;
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

            _productRepository.Add(product);
            _uow.SaveChanges();

        }
        public void AddToWareHouse(WareHouseViewModel model)
        {
            WareHouse product = new WareHouse
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                Mass = model.Mass,
                Unitofmass = model.Unitofmass,
                Balance = model.Balance,
                Count = model.Count,
                DateBuy = model.DateBuy,
                Productid = model.ProductId

            };

            _Wrepo.Add(product);
            _uow.SaveChanges();

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
                WareHouseId = product.WareHouseId
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
        public async Task<ProductViewModel> GetProductByIdAsync(int productId)
        {
            var product =  await _productRepository.GetProductByIdAsync(productId);
            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                unitOfmass = product.Unitofmass,
                Mass = product.Mass,
                Count = product.Count,
                SellerId = product.SellerId        
            };
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

        public List<Store> GetStores()
        {
            var data = _store.GetAll();
            List<Store> store = data.Select(product => new Store
            {
                Id = product.Id,
                Name = product.Name          
            }).ToList();
            return store;
        }
    }
}
