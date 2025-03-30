
using Accounting_Software.Data.Entites;
using Accounting_Software.ViewModel;

namespace Accounting_Software.Service_Interfaces
{
    public interface IStoreProductService
    {
        //StoreProductViewModel GetIdByStoreAndProduct(int storeId, int productId);
        //IEnumerable<StoreProductViewModel> GetByStoreId(int storeId);    
        void Add(StoreProductViewModel storeProduct);
        StoreProductViewModel Update(StoreProductViewModel storeProduct);
        void Delete(StoreProductViewModel model);
        List<StoreProductViewModel> GetStores();
        StoreProduct GetStoreProduct(int storeId, int productId);
        void AddProductToStore(int productId, int storeId);
        List<StoreProductViewModel> GetAll();
        List<StoreProductViewModel> GetProductByStoreId(int? sellerId);
        void GetBalanceSale(int id);
        StoreProductViewModel GetById (int storeId);
    }
}
