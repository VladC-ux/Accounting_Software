
using Accounting_Software.Data.Entities;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace Accounting_Software.Controllers
{
    [Authorize]
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
            var user = _userService.GetAll().FirstOrDefault();
            ViewBag.SellerId = SellerId;
            ViewBag.UserBalance = user?.Balance ?? 0;
            ViewBag.Products = _productService.GetProductsBySellerId(SellerId);
            return View(new ProductViewModel { SellerId = SellerId });
        }

        [HttpPost]
        public IActionResult Add(ProductViewModel product)
        {
            var user = _userService.GetAll().FirstOrDefault();
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
            }
            else
            {
                try
                {
                    _productService.Add(product, user.Id);
                    TempData["SuccessMessage"] = $"'{product.Name}' added successfully!";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }

            ModelState.Clear();
            var updatedUser = _userService.GetAll().FirstOrDefault();
            ViewBag.SellerId = product.SellerId;
            ViewBag.UserBalance = updatedUser?.Balance ?? 0;
            ViewBag.Products = _productService.GetProductsBySellerId(product.SellerId);
            return View(new ProductViewModel { SellerId = product.SellerId });
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
        public IActionResult Edit(int Id)
        {
           
            var data = _productService.GetById(Id);
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
