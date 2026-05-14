using Accounting_Software.Data.Entities;
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

        public void Add(TransactionHistoryViewModel transactionHistoryViewModel)
        {
            TransactionHistory transactionHistory = new TransactionHistory
            {
                Id = transactionHistoryViewModel.Id,
                UserId = transactionHistoryViewModel.UserId,
                ProductName = transactionHistoryViewModel.ProductName,
                Count = transactionHistoryViewModel.Count,
                Price = transactionHistoryViewModel.Price,
                SoldDate = transactionHistoryViewModel.SoldDate,
                Description = transactionHistoryViewModel.Description,
                Mass = transactionHistoryViewModel.Mass,
                unitOfmass = transactionHistoryViewModel.unitOfmass,
                typeofAction = transactionHistoryViewModel.typeofAction,
                StoreName = transactionHistoryViewModel.StoreName
            };
            _transactionHistoryRepository.Add(transactionHistory);
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
                typeofAction = transactionHistory.typeofAction,
                StoreName = transactionHistory.StoreName               
            }).ToList();

            return transactionHistoryViewModels;

        }
    }
}
