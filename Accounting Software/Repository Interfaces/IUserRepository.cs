using Accounting_Software.Data.Entites;

namespace Accounting_Software.Repository_Interfaces
{
    public interface IUserRepository
    {
        decimal GetBalance(int Id);
        User GetUserById(int Id);
        bool Add(User user);
    }
}
