using Accounting_Software.Data.Entites;
using Accounting_Software.Repositories;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.UnitOfWorkk;
using Accounting_Software.ViewModel;

namespace Accounting_Software.Service
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _sp;
        private readonly IUnitofWork _uow;
        public StoreService(IStoreRepository storeproduct, IUnitofWork uow)
        {
            _sp = storeproduct;
            _uow = uow;
        }
        public void Add(Store store)
        {
            Store st = new Store
            {
                Id = store.Id,
                Name = store.Name,
            };
            _sp.Add(st);
            _uow.SaveChanges();
        }
        public void Delete(int id)
        {
            var st = GetById(id);
            if (st == null)
            {
                throw new KeyNotFoundException("Store not found.");
            }

            _sp.Delete(id);
            _uow.SaveChanges();
        }

        public List<Store> GetAll()
        {
            var data = _sp.GetAll();

            if (data == null)
            {
                data = new List<Store>();
            }

            return data.Select(store => new Store
            {
                Id = store.Id,
                Name = store.Name,
            }).ToList();
        }

        public Store GetById(int id)
        {
            var store = _sp.GetById(id);
            if (store == null)
            {
                throw new KeyNotFoundException("Store not found.");
            }
            return store;
        }

        public void Update(Store store)
        {
            Store st = new Store
            {
                Id = store.Id,
                Name = store.Name,
            };
            _sp.Update(st);
            _uow.SaveChanges();
        }
    }
}
