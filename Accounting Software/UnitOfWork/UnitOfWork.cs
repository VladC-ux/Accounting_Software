using Accounting_Software.Data;
using Microsoft.AspNetCore.Authentication;

namespace Accounting_Software.UnitOfWorkk
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

    }
}
