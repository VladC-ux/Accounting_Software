using Accounting_Software.Data.Entites;
namespace Accounting_Software.Repository_Interfaces
{
    public interface IProductRepositoryInterface
    {
        void Add(Product Product);
        Product Update(Product Product);
        void Delete(Product Product);   
        List<Product> GetAll();
        Product GetById(int id);


        
        
         

    }
}
