using Accounting_Software.Data.Entities;

namespace Accounting_Software.Repository_Interfaces
{
    public interface IInvoiceRepository
    {
        void Add(Invoice invoice);
        Invoice? GetById(int id);
        List<Invoice> GetAll();
        List<Invoice> GetByUserId(int userId);
        int CountForYear(int year);
    }
}
