using Accounting_Software.Date.Entites;
using Accounting_Software.ViewModel;

namespace Accounting_Software.Service_Interfaces
{
    public interface ISellerService
    {
        void Add(SellerViewModel user);
        void Update(SellerViewModel model);
        List<SellerViewModel> GetAll();
        SellerViewModel GetById(int id);
        void Delete(int id);
    }
}
