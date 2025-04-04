using Accounting_Software.Data.Entites;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;

namespace Accounting_Software.Service
{
    public class TransactionHistoryService:ITransactionHistoryService
    {
        private readonly ITransactionHistoryRepository _transactionHistoryRepository;
        public TransactionHistoryService(ITransactionHistoryRepository transactionHistoryRepository)
        {
            _transactionHistoryRepository = transactionHistoryRepository;
        }

        public void Add(TransactionHistoryViewModel transationHistoryViewModel)
        {
            TransactionHistory transactionHistory = new TransactionHistory
            {
                Id = transationHistoryViewModel.Id,
                UserId = transationHistoryViewModel.UserId,
                ProductName = transationHistoryViewModel.ProductName,
                Count = transationHistoryViewModel.Count,
                Price = transationHistoryViewModel.Price,
                SoldDate = transationHistoryViewModel.SoldDate,
                Description = transationHistoryViewModel.Description,
                Mass = transationHistoryViewModel.Mass,
                unitOfmass = transationHistoryViewModel.unitOfmass,  
            };
        }

        public void Delete(int id)
        {
            var data = _transactionHistoryRepository.GetById(id);
            _transactionHistoryRepository.Delete(data.Id);

        }

        public List<TransactionHistoryViewModel> GetHistoryByUserId(int Id)
        {
           var data = _transactionHistoryRepository.GetHistoryByUserId(Id);

            List<TransactionHistoryViewModel> transactionHistoryViewModels = data.Select(transactionHistory => new TransactionHistoryViewModel
            {
                Id = transactionHistory.Id,
                UserId = transactionHistory.UserId,
                ProductName = transactionHistory.ProductName,
                Count = transactionHistory.Count,
                Price = transactionHistory.Price,
                SoldDate = transactionHistory.SoldDate,
                Description = transactionHistory.Description,
                Mass = transactionHistory.Mass,
                unitOfmass = transactionHistory.unitOfmass,

            }).ToList();

            return transactionHistoryViewModels;

        }
    }
}
