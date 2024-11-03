using Accounting_Software.Data.Entites;

namespace Accounting_Software.Repository_Interfaces
{
    public interface IStoreProductRepository
    {
        StoreProduct GetById(int storeId, int productId);
        IEnumerable<StoreProduct> GetByStoreId(int storeId);
        void Add(StoreProduct storeProduct);
        void Update(StoreProduct storeProduct);
        void Delete(int storeId, int productId);
        Task AddAsync(StoreProduct storeProduct);

    }
}
