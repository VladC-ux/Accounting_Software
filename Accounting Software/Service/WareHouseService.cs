using Accounting_Software.Repositories;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.UnitOfWorkk;
using Accounting_Software.ViewModel;

namespace Accounting_Software.Service
{
    public class WareHouseService : IWareHouseServiceInterface
    {
        private readonly IWareHouseServiceInterface _whrepo;
        private readonly IUnitofWork _uow;

        public WareHouseService(IWareHouseServiceInterface repository, IUnitofWork uow)
        {
            _whrepo = repository;
            _uow = uow;
        }

        public void Add(ProductViewModel product)
        {
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

            List<ProductViewModel> ProductViewModel = data.Select(s => new ProductViewModel
            {
                Id = s.Id,
                Name = s.Name,


            }).ToList();

            return data;
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
