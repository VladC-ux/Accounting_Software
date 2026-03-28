using Accounting_Software.Data;
using Microsoft.AspNetCore.Authentication;

namespace Accounting_Software.UnitOfWork
{
    public class UnitOfWork : IUnitofWork
    {

        private readonly DBContextAccounting _context;
        public UnitOfWork(DBContextAccounting context)
        {
            _context = context;
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

       
    }
}
