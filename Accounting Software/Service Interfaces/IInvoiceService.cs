using Accounting_Software.ViewModel;

namespace Accounting_Software.Service_Interfaces
{
    public interface IInvoiceService
    {
        InvoiceViewModel Create(InvoiceCreateViewModel model, int userId);
        InvoiceViewModel? GetById(int id);
        List<InvoiceViewModel> GetAll();
        List<InvoiceViewModel> GetByUserId(int userId);
    }
}
