using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Accounting_Software.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductServiceInterface _productService;

        public ProductController(IProductServiceInterface productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            var list = _productService.GetAll();
            return View(list);
        }


        
        public IActionResult Add(ProductViewModel product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    GetDropDownData();
                    return View(product);
                }

                _productService.Add(product);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error adding product: {ex.Message}");
                throw;
            }
        }



        private void GetDropDownData()
        {
            ViewBag.Products = _productService.GetAll();
        }
        private void GetDropDownEnum()
        {
           
           
        }
    }
}
