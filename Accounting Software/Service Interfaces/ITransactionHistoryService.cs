using Accounting_Software.ViewModel;

namespace Accounting_Software.Service_Interfaces
{
    public interface ITransactionHistoryService
    {
        void Add(TransactionHistoryViewModel transactionHistory);
        void Delete(int id);
        List<TransactionHistoryViewModel> GetHistoryByUserId(int Id);  

        
    }
}
