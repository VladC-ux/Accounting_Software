
using Accounting_Software.Date.Entites;
using Accounting_Software.ViewModel;

namespace Accounting_Software.Repository_Interfaces
{
    public interface IUserRepositoryInterface
    {
        void Add(User user);
        User Update(User user); 

        List<User> GetAll();

        User GetById(int id);

        void Delete(int id);
       
    }
}
