using Accounting_Software.Data.Entites;
using Accounting_Software.Date.Entites;

namespace Accounting_Software.Repository_Interfaces
{
    public interface IProductRepositoryInterface
    {
        void Add(Product product);
        Product Update(Product product);
        List<Product> GetProductsBySellerId(int sellerId);
        void Delete(int id);   
        List<Product> GetAll();
        Product GetById(int id);
        Product GetWareHouseViewModelById(int id);

        void AddToWareHouse(WareHouse product);









    }
}
