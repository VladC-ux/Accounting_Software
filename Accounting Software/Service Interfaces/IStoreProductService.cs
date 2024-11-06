
using Accounting_Software.Data.Entites;
using Accounting_Software.ViewModel;

namespace Accounting_Software.Service_Interfaces
{
    public interface IStoreProductService
    {
        StoreProductViewModel GetById(int storeId, int productId);
        IEnumerable<StoreProductViewModel> GetByStoreId(int storeId);
        void Add(StoreProductViewModel storeProduct);
        void Update(StoreProductViewModel storeProduct);
        void Delete(int storeId, int productId);
        void AddProductToStore(int productId, int storeId);
        List<StoreProductViewModel> GetStores();

        StoreProduct GetStoreProduct(int storeId, int productId);
    }
}
