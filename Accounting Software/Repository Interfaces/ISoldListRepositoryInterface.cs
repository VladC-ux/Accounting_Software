using Accounting_Software.Data.Entites;

namespace Accounting_Software.Repository_Interfaces
{
    public interface ISoldListRepositoryInterface
    {

        List<SoldList> GetAllSoldItems();

        void GetById(int id);
    }
}
