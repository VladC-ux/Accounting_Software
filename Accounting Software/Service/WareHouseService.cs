using Accounting_Software.Data.Entites;
using Accounting_Software.Date.Entites;
using Accounting_Software.Repositories;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.UnitOfWorkk;
using Accounting_Software.ViewModel;
using Microsoft.CodeAnalysis;

namespace Accounting_Software.Service
{
    public class WareHouseService : IWareHouseServiceInterface
    {
        private readonly IWareHouseRepositoryInterface _whrepo;
        private readonly IUnitofWork _uow;

        public WareHouseService(IWareHouseRepositoryInterface repository, IUnitofWork uow)
        {
            _whrepo = repository;
            _uow = uow;
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

            _whrepo.Add(product);
            _uow.SaveChanges();

        }


        public void Delete(int id)
        {
            _whrepo.Delete(id);
        }

        public List<ProductViewModel> GetAll()
        {


            var data = _whrepo.GetAll();

            List<ProductViewModel> ProductViewModel = data.Select(product => new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                unitOfmass = product.Unitofmass,
                Mass = product.Mass



            }).ToList();

            return ProductViewModel;
        }

        public double GetAveragePrice()
        {
            throw new NotImplementedException();
        }

        public ProductViewModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ProductViewModel GetByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
