using Accounting_Software.Data.Entites;
using Accounting_Software.Date.Entites;
using Accounting_Software.Repositories;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.UnitOfWorkk;
using Accounting_Software.ViewModel;
using Microsoft.EntityFrameworkCore;

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
                WareHouseId = model.WareHosueId

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

            List<ProductViewModel> productviewmodel = data.Select(seller => new ProductViewModel
            {
                Id = seller.Id,
                Name = seller.Name,

            }).ToList();

            return productviewmodel;
        }


        public ProductViewModel GetById(int id)
        {
            var data = _whrepo.GetById(id);
            return new ProductViewModel
            {
                Id = data.Id,
                Name = data.Name,
                Price = data.Price,
                Description = data.Description,
                Mass = data.Mass,
                SellerId = data.SellerId

            };
        }
        public void AddToWareHouse(WareHouseViewModel model)
        {
            WareHouse warehouse = new WareHouse
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                Mass = model.Mass,
                Unitofmass = model.Unitofmass,
                Balance = model.Balance,
                Count = model.Count,
                DateBuy = model.DateBuy

            };

            _whrepo.AddToWareHouse(warehouse);
            _uow.SaveChanges();
        }



        public double GetAveragePrice()
        {
            throw new NotImplementedException();
        }

      

        public ProductViewModel GetByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
