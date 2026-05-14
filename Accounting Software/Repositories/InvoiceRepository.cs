using Accounting_Software.Data;
using Accounting_Software.Data.Entities;
using Accounting_Software.Repository_Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Accounting_Software.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly DBContextAccounting _context;

        public InvoiceRepository(DBContextAccounting context)
        {
            _context = context;
        }

        public void Add(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
        }

        public Invoice? GetById(int id)
        {
            return _context.Invoices
                .Include(i => i.Items)
                .Include(i => i.Store)
                .Include(i => i.User)
                .FirstOrDefault(i => i.Id == id);
        }

        public List<Invoice> GetAll()
        {
            return _context.Invoices
                .Include(i => i.Items)
                .Include(i => i.Store)
                .Include(i => i.User)
                .OrderByDescending(i => i.IssuedAt)
                .ToList();
        }

        public List<Invoice> GetByUserId(int userId)
        {
            return _context.Invoices
                .Include(i => i.Items)
                .Include(i => i.Store)
                .Where(i => i.UserId == userId)
                .OrderByDescending(i => i.IssuedAt)
                .ToList();
        }

        public int CountForYear(int year)
        {
            return _context.Invoices.Count(i => i.IssuedAt.Year == year);
        }
    }
}
