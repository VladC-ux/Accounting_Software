using Accounting_Software.Date.Entites;
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

        [HttpGet]
        public IActionResult Add(int SellerId)
        {

            ViewBag.SellerId = SellerId;
            return View();
        }
        [HttpPost]
        public IActionResult Add(ProductViewModel product)
        {

            GetDropDownData();
            _productService.Add(product);
            return View(product);

        }
        [HttpGet]
        public IActionResult ShowSellerProduct(int SellerId)
        {
            ViewBag.SellerId = SellerId;
            return View();
        }
        [HttpPost]
        public IActionResult ShowSellerProduct()
        {
            var data =  _productService.GetByIdShowProduct();
            return View(data);
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
