using Accounting_Software.ViewModel;

namespace Accounting_Software.Service_Interfaces
{
    public interface ITaxService
    {
        TaxOverviewViewModel GetOverview(int userId);
        void Add(TaxRuleViewModel model);
        void Update(TaxRuleViewModel model);
        void Delete(int id);
        TaxRuleViewModel? GetById(int id);
    }
}
