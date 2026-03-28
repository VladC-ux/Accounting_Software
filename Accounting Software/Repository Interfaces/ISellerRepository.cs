
using Accounting_Software.Data.Entities;
using Accounting_Software.ViewModel;

namespace Accounting_Software.Repository_Interfaces
{
    public interface ISellerRepository
    {
        void Add(Seller user);
        Seller Update(Seller user); 
        List<Seller> GetAll(); 
        Seller GetById(int id);
        void Delete(int id);
        Seller GetSellerName(string name);
    }
}
