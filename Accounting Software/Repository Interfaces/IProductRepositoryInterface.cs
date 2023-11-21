using Accounting_Software.Data.Entites;
namespace Accounting_Software.Repository_Interfaces
{
    public interface IProductRepositoryInterface
    {
        void Add(Product Product);
        Product Update(Product Product);
        void Delete(int id);   
        List<Product> GetAll();
        Product GetById(int id);

      





    }
}
