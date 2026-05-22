using Accounting_Software.Data.Entities;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.UnitOfWork;
using Accounting_Software.ViewModel;

namespace Accounting_Software.Service
{
    public class TaxService : ITaxService
    {
        private readonly ITaxRepository _taxRepository;
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly IUnitofWork _uow;

        public TaxService(
            ITaxRepository taxRepository,
            ITransactionHistoryService transactionHistoryService,
            IUnitofWork uow)
        {
            _taxRepository = taxRepository;
            _transactionHistoryService = transactionHistoryService;
            _uow = uow;
        }

        public TaxOverviewViewModel GetOverview(int userId)
        {
            var transactions = _transactionHistoryService.GetHistoryByUserId(userId);

            decimal totalIncome = transactions
                .Where(t => t.typeofAction == "Sale")
                .Sum(t => t.Total);
            decimal totalExpenses = transactions
                .Where(t => t.typeofAction == "Add")
                .Sum(t => t.Total);

            var rules = _taxRepository.GetByUserId(userId)
                .Select(r => new TaxRuleViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Rate = r.Rate,
                    Description = r.Description,
                    UserId = r.UserId,
                    Amount = Math.Round(totalIncome * r.Rate / 100m, 2)
                })
                .ToList();

            return new TaxOverviewViewModel
            {
                UserId = userId,
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                NetProfit = totalIncome - totalExpenses,
                Rules = rules
            };
        }

        public void Add(TaxRuleViewModel model)
        {
            var rule = new TaxRule
            {
                Name = model.Name,
                Rate = model.Rate,
                Description = model.Description,
                UserId = model.UserId
            };
            _taxRepository.Add(rule);
            _uow.SaveChanges();
        }

        public void Update(TaxRuleViewModel model)
        {
            var rule = new TaxRule
            {
                Id = model.Id,
                Name = model.Name,
                Rate = model.Rate,
                Description = model.Description,
                UserId = model.UserId
            };
            _taxRepository.Update(rule);
            _uow.SaveChanges();
        }

        public void Delete(int id)
        {
            _taxRepository.Delete(id);
            _uow.SaveChanges();
        }

        public TaxRuleViewModel? GetById(int id)
        {
            var rule = _taxRepository.GetById(id);
            if (rule == null)
                return null;

            return new TaxRuleViewModel
            {
                Id = rule.Id,
                Name = rule.Name,
                Rate = rule.Rate,
                Description = rule.Description,
                UserId = rule.UserId
            };
        }
    }
}
