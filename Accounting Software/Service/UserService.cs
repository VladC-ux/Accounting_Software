using Accounting_Software.Data.Entites;
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
        public void Add(UserViewModel userview)
        {          
            User user = new User
            {
                Name = userview.Name, 
                Balance = userview.Balance,
            };
            _userRepository.Add(user);        
        }

        public List<UserViewModel> GetAll()
        {
            var data = _userRepository.GetAll();
            List<UserViewModel> userviewmodel = data.Select(userviewmodel => new UserViewModel
            {
                Id = userviewmodel.Id,

            }).ToList();
            return userviewmodel;
        }

        public UserViewModel GetBalance(int id)
        {        
            var user = _userRepository.GetUserById(id);
       
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} was not found.");
            }

            return new UserViewModel
            {
                Id = user.Id,
                Balance = user.Balance,
                Name = user.Name
            };
        }

        public UserViewModel GetUserById(int id)
        {
            var data = _userRepository.GetUserById(id);
            return new UserViewModel
            {
                Id = data.Id
            };
        }

     
    }
}
