using Accounting_Software.Data.Entites;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Accounting_Software.UnitOfWorkk;
using Microsoft.CodeAnalysis;

namespace Accounting_Software.Service
{
    public class ProductService : IProductServiceInterface
    {
        private readonly IProductRepositoryInterface _productRepository;
        private readonly IUnitofWork _uow;


        public ProductService(IProductRepositoryInterface poroductrepository, IUnitofWork uow)
        {
            _productRepository = poroductrepository;
            _uow = uow;

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
                SellerId = model.SellerId

            };

            _productRepository.Add(product);
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


        public ProductViewModel GetById(int model)
        {
            var data = _productRepository.GetById(model);
            return new ProductViewModel
            {
                Id = data.Id,
                Name = data.Name,
                Price = data.Price,
                Description = data.Description,
                Mass = data.Mass
            };

        }

        public List<ProductViewModel> GetByIdShowProduct()
        {
            var data = _productRepository.GetAll();
            List<ProductViewModel> productview = data.Select(product => new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Mass = product.Mass
            }).ToList();

            return productview;
        }

        public void Update(ProductViewModel Product)
        {
            Product product = new Product
            {
                Id = Product.Id,
                Name = Product.Name,
                Price = Product.Price,
                Description = Product.Description,

            };
            var data = _productRepository.Update(product);
            _uow.SaveChanges();
        }
    }
}
