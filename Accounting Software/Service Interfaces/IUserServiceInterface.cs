using Accounting_Software.Date.Entites;
using Accounting_Software.ViewModel;

namespace Accounting_Software.Service_Interfaces
{
    public interface IUserServiceInterface
    {
        void Add(AddEditViewModel user);
        void Update(UserViewModel model);

        List<UserViewModel> GetAll();

        UserViewModel GetById(int id);

        void Delete(UserViewModel id);
    }
}
