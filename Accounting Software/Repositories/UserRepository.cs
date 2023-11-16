using Accounting_Software.Data;
using Accounting_Software.Date.Entites;
using Accounting_Software.Repository_Interfaces;
using Microsoft.Identity.Client;

namespace Accounting_Software.Repositories
{
    public class UserRepository : IUserRepositoryInterface
    {
        private readonly DBContextAccounting _context;

        public UserRepository(DBContextAccounting context)
        {
            _context = context;
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public void Delete(int id)
        {
            var entityToDelete = _context.Users.Find(id);

            if (entityToDelete != null)
            {
                _context.Users.Remove(entityToDelete);
                
            }
        }
        public List<User> GetAll()
        {
            var entity = _context.Users.ToList();
            return entity;
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User Update(User user)
        {
            var entity = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (entity != null)
            {
                entity.Name = user.Name;
                _context.Users.Update(entity);
                _context.SaveChanges();

                return entity;
            }
            else
            {
                throw new NotImplementedException("The User is not found");
            }
        }
    }
}
