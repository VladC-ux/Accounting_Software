using Accounting_Software.Data.Entites;
using Accounting_Software.Date.Entites;
using Accounting_Software.Service;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace Accounting_Software.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ISellerService _sellerService;
        private readonly IWareHouseService _warehouse;

        public ProductController(IProductService productService, ISellerService sellerService, IWareHouseService warehouse)
        {
            _productService = productService;
            _sellerService = sellerService;
            _warehouse = warehouse;
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
            try
            {
                _productService.Add(product);

                TempData["SuccessMessage"] = "Your product is successfuly add!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error with add product : {ex.Message}";
            }

            return View(product);
        }

        [HttpGet]
        public void AddToWareH(int Id)
        {
            _productService.GetById(Id);
        }
        [HttpPost]
        public IActionResult AddToWareH(WareHouseViewModel model)
        {
            _productService.AddToWareHouse(model);
            return RedirectToAction("ShowSellerProduct");
        }

        [HttpGet]
        public IActionResult ShowSellerProduct(int? SellerId)
        {
            var data = _productService.GetProductsBySellerId(SellerId);
            return View(data);
        }

        [HttpPost]
        public IActionResult ShowSellerProduct()
        {
            var data = _productService.GetAll();
            return View(data);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var data = _productService.GetById(id);
            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(ProductViewModel model)
        {
            _productService.Update(model);
            return RedirectToAction("ShowSellerProduct", new { SellerId = model.SellerId });
        }
        public IActionResult Delete(ProductViewModel model)
        {
            _productService.Delete(model);
            return RedirectToAction("Index", "Seller");
        }     
        public IActionResult ShowShops()
        {
            var data = _productService.GetStores();
            return View(data);
        }

        private void GetDropDownData()
        {
            ViewBag.ProductId = _productService.GetAll();
        }
        private void GetDropDownSeller()
        {
            ViewBag.Sellers = _sellerService.GetAll();

        }
    }
}
