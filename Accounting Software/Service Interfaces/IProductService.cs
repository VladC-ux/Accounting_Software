using Accounting_Software.Data.Entities;
using Accounting_Software.ViewModel;

namespace Accounting_Software.Service_Interfaces
{
    public interface IProductService
    {
        void Add(ProductViewModel Product,int id);
        List<ProductViewModel> GetProductsBySellerId(int? sellerId);
        void Update(ProductViewModel Product);
        void Delete(ProductViewModel Product);
        List<ProductViewModel> GetAll();
        ProductViewModel GetById(int id);       
   
    }        
}
