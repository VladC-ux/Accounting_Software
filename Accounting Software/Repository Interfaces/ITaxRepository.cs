using Accounting_Software.Data.Entities;

namespace Accounting_Software.Repository_Interfaces
{
    public interface ITaxRepository
    {
        void Add(TaxRule rule);
        TaxRule? Update(TaxRule rule);
        List<TaxRule> GetByUserId(int userId);
        TaxRule? GetById(int id);
        void Delete(int id);
    }
}
