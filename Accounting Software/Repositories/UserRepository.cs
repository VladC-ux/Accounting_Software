using Accounting_Software.Data;
using Accounting_Software.Data.Entites;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.UnitOfWorkk;

namespace Accounting_Software.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContextAccounting _context;
        private readonly IUnitofWork _uow;

        public UserRepository(DBContextAccounting context, IUnitofWork uow)
        {
            _context = context;
            _uow = uow;
        }

        public bool Add(User user)
        {
            if (_context.Users.Any())
            {
                return false;
            }
            _context.Users.Add(user);
            _uow.SaveChanges();
            return true;
    
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public decimal GetBalance(int userId)
        {
            var user = _context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefault();

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} was not found.");
            }

            return user.Balance;
        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public int UserCount()
        {
            return _context.Users.Count();
        }
    }
}
