using Accounting_Software.Data.Entites;

namespace Accounting_Software.Repository_Interfaces
{
    public interface IStoreProductRepository
    {
        StoreProduct GetIdByStoreAndProduct(int storeId, int productId);
        StoreProduct GetById(int storeId);
        void Add(StoreProduct storeProduct);
        StoreProduct Update(StoreProduct storeProduct);
        void Delete(StoreProduct Id);       
        Task AddAsync(StoreProduct storeProduct);
        List<StoreProduct> GetStores();
        StoreProduct GetStoreProduct(int storeId, int productId);
        IEnumerable<StoreProduct> GetStoreProducts();
        List<StoreProduct> GetAll();
        List<StoreProduct> GetProductByStoreId(int storeId);
        List<string> GetSellerName();
       

    }
}
