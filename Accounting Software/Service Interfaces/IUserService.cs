using Accounting_Software.ViewModel;
using Humanizer.Localisation.TimeToClockNotation;

namespace Accounting_Software.Service_Interfaces
{
    public interface IUserService
    {
        UserViewModel GetBalance(int Id);

        void Add(UserViewModel user);
    }
}
