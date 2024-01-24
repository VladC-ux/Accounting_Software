using Accounting_Software.Data.Entites;
using Accounting_Software.Date.Entites;
using Accounting_Software.ViewModel;

namespace Accounting_Software.Service_Interfaces
{
    public interface IWareHouseServiceInterface
    {
        void Add(ProductViewModel product);
        void Delete(int id);
        List<ProductViewModel> GetAll();
        ProductViewModel GetById(int id);
        ProductViewModel GetByName(string name);
        double GetAveragePrice();

        void AddToWareHouse(WareHouseViewModel wareHouse);
    }


}

