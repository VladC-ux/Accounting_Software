using Accounting_Software.Data.Entites;

namespace Accounting_Software.Repository_Interfaces
{
    public interface ISoldListRepository
    {

        List<SoldList> GetAllSoldItems();

        void GetById(int id);
    }
}
