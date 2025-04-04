using Accounting_Software.Data;
using Accounting_Software.Data.Entites;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.UnitOfWorkk;

namespace Accounting_Software.Repositories
{
    public class TransactionHistoryRepository:ITransactionHistoryRepository
    {
        public readonly DBContextAccounting _context;
        public readonly IUnitofWork _uow;

        public TransactionHistoryRepository(DBContextAccounting context,IUnitofWork uow)
        {
            _context = context;
            _uow = uow;
        }

        public void Add(TransactionHistory transactionHistory)
        {
           _context.TransactionHistories.Add(transactionHistory);
            _uow.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.Remove(id);
            _uow.SaveChanges();
        }

        public TransactionHistory GetById(int id)
        {
            return _context.TransactionHistories.FirstOrDefault(x=>x.Id==id);
        }




        public List<TransactionHistory> GetHistoryByUserId(int userId)
        {
            return _context.TransactionHistories.Where(t => t.UserId == userId)//lyambda expression
                 .OrderByDescending(t => t.SoldDate).ToList();
        }
    }
}
