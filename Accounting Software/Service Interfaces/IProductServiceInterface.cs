using Accounting_Software.Data.Entites;
using Accounting_Software.ViewModel;

namespace Accounting_Software.Service_Interfaces
{
    public interface IProductServiceInterface
    {
        void Add(ProductViewModel Product);
        List<ProductViewModel> GetProductsBySellerId(int? sellerId);

        void Update(ProductViewModel Product);
        void Delete(ProductViewModel Product);
        List<ProductViewModel> GetAll();
        ProductViewModel GetById(int id);

    

    }        
}
