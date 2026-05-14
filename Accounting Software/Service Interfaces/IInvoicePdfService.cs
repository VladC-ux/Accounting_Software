using Accounting_Software.ViewModel;

namespace Accounting_Software.Service_Interfaces
{
    public interface IInvoicePdfService
    {
        byte[] Generate(InvoiceViewModel invoice);
    }
}
