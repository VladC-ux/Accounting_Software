﻿using Accounting_Software.Date.Entites;
using Accounting_Software.Service;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Accounting_Software.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductServiceInterface _productService;
        private readonly ISellerServiceInterface _sellerService;
        private readonly IWareHouseServiceInterface _warehouse;

        public ProductController(IProductServiceInterface productService, ISellerServiceInterface sellerService, IWareHouseServiceInterface warehouse)
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
        public IActionResult AddToWareH(int ProductId)
        {
            ViewBag.ProductId = ProductId;
            return View();
        }
        [HttpPost]
        public IActionResult AddtoWareH(WareHouseViewModel product)
        {
            _warehouse.AddToWareHouse(product);
            return View(product);
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
        //[HttpGet]
        //public IActionResult MoveToWare(int id)
        //{

        //}
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
