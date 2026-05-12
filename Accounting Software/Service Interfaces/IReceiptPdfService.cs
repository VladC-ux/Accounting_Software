using Accounting_Software.Data.Entities;

namespace Accounting_Software.Service_Interfaces
{
    public interface IReceiptPdfService
    {
        byte[] GenerateSaleReceipt(TransactionHistory transaction, string? buyerName);
    }
}
