
using Accounting_Software.Data.Entites;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ISellerService _sellerService;
        private readonly IUserService _userService;
      
        public ProductController(IProductService productService, ISellerService sellerService,IUserService userservice)
        {
            _productService = productService;
            _sellerService = sellerService;
            _userService = userservice;
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
            ViewBag.Users = _userService.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Add(ProductViewModel product,int userid)
        {
            try
            {
                _productService.Add(product,userid);
                TempData["SuccessMessage"] = "Your product is successfuly add!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error with add product : {ex.Message}";
            }

            ModelState.Clear(); 
            return View(new ProductViewModel()); 
        }

        [HttpGet]
        public IActionResult ShowSellerProduct(int? SellerId)
        {
            ViewBag.SellerId = _sellerService.GetAll();
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
            var data = _productService.GetById(model.Id);
            _productService.Delete(data);
            return RedirectToAction("ShowSellerProduct", new { SellerId = model.SellerId });
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
