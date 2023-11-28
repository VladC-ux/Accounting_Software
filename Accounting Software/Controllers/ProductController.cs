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

        [Route("Product/Add/{SellerId}")]
        public IActionResult Add(int SellerId)
        {
            ViewBag.SellerId = SellerId;
            return View();
        }
        public IActionResult Add(ProductViewModel product)
        {
            
                if (!ModelState.IsValid)
                {
                    GetDropDownData();
                    return View(product);
                }

                _productService.Add(product);
                return RedirectToAction("Index");
            

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
