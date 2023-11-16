using Accounting_Software.Data.Entites;

namespace Accounting_Software.Repository_Interfaces
{
    public interface ISellerRepositoryInterface
    {
        void Add(Seller Seller);
        Seller Update(Seller Seller);
        void Delete(Seller Seller);

        Seller GetById(int id);

        List<Seller> GetAll();

    }
}
