using Accounting_Software.Data;
using Accounting_Software.Data.Entites;

namespace Accounting_Software.Repository_Interfaces
{
    public interface ITransactionHistoryRepository
    {
        void Add(TransactionHistory transactionHistory);
        void Delete(int id);
        List<TransactionHistory> GetHistoryByUserId(int userId);

    }
}
