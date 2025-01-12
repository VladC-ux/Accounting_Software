using Accounting_Software.Data;
using Accounting_Software.Data.Entites;
using Accounting_Software.Repository_Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Accounting_Software.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContextAccounting _context;

        public UserRepository(DBContextAccounting context)
        {
            _context = context;
        }
        public decimal GetBalance(int userId)
        {
            var balance = _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Balance)
                .FirstOrDefault();

            return balance;
        }

    }
}
