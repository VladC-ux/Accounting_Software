using Accounting_Software.Data.Entites;
using Accounting_Software.Date.Entites;

namespace Accounting_Software.Repository_Interfaces
{
    public interface IProductRepositoryInterface
    {
        void Add(Product product);
        Product Update(Product product);
        List<Product> GetByIdShowProduct();
        void Delete(int id);   
        List<Product> GetAll();
        Product GetById(int id);

      





    }
}
