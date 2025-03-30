using Accounting_Software.Data.Entites;
using Accounting_Software.ViewModel;

namespace Accounting_Software.Service_Interfaces
{
    public interface IStoreService
    {
        Store GetById(int id);
        List<Store> GetAll();
        void Add(Store store);
        void Update(Store store);
        void Delete(int id);
    }
}