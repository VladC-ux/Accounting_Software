using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;

namespace Accounting_Software.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public UserViewModel GetBalance(int id)
        {
            
            var balance = _userRepository.GetBalance(id);
            var user = _userRepository.GetUserById(id);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} was not found.");
            }

            return new UserViewModel
            {
                Id = user.Id,
                Balance = balance 
            };
        }

    }
}
