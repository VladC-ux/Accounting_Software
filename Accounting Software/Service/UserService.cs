using Accounting_Software.Repository_Interfaces;

namespace Accounting_Software.Service
{
    public class UserService : IUserRepository
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public decimal GetBalance(int Id)
        {
            var data = _userRepository.GetBalance(Id);
            return data;
        }
    }
}
