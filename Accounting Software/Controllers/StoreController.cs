using Accounting_Software.Data.Entites;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Accounting_Software.Controllers
{
    public class StoreController : Controller
    {
        private readonly IStoreService _storeService;
        private readonly IStoreProductService _storeProductService;
        private readonly IProductService _productService;
        private readonly IStoreProductRepository _storeProductRepository;
        private readonly ISellerService _sellerService;


        public StoreController(IStoreService storeService, IStoreProductService storeproduct, IStoreProductRepository storeProductRepository, IProductService productService, ISellerService sellerService)
        {
            _storeService = storeService;
            _storeProductService = storeproduct;
            _storeProductRepository = storeProductRepository;
            _productService = productService;
            _sellerService = sellerService;
        }

        public IActionResult Index()
        {
            var stores = _storeService.GetAll();
            return View(stores);
        }

        public IActionResult Add(Store store)
        {
            if (ModelState.IsValid)
            {
                _storeService.Add(store);
                return RedirectToAction("Index");
            }
            return View(store);
        }
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                _storeService.Delete(id);
                return RedirectToAction("Index");
            }
            return View(id);
        }
        public IActionResult Edit(Store store)
        {
            if (ModelState.IsValid)
            {
                _storeService.Update(store);
                return RedirectToAction("Index");
            }
            return View(store);
        }
        //public IActionResult ProductToStore(int productId, int storeId)
        //{a
        //    _storeProductService.AddProductToStore(productId, storeId);
        //    return View();
        //}

        [HttpGet]
        public IActionResult ShowShops(int id)
        {
            ViewBag.ProductId = id;
            var stores = _storeService.GetAll();
            var storeProducts = _storeProductRepository.GetAll();

            var storeProductViewModels = stores.Select(sp => new StoreProductViewModel
            {
                StoreId = sp.Id,
                StoreName = sp.StoreName,
            }).ToList();

            return View(storeProductViewModels);
        }

        [HttpGet]
        public IActionResult AddProductToStore(int storeId, int productId)
        {
            var store = _storeService.GetById(storeId);
            if (store == null)
            {
                return NotFound("Shop not found.");
            }

            var product = _productService.GetById(productId);
            if (product == null)
            {
                return NotFound("Product not found");
            }


            var viewModel = new StoreProductViewModel
            {
                Id = product.Id,
                StoreId = store.Id,
                ProductId = product.Id,
                StoreName = store.StoreName,
                ProductName = product.Name,
                Price = product.Price,
                Count = product.Count,
                unitOfmass = product.unitOfmass,
                Description = product.Description,
                Mass = product.Mass,
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddProductToStore(StoreProductViewModel model)
        {
            var storeProduct = new StoreProductViewModel
            {
                Id = model.Id,
                StoreId = model.StoreId,
                ProductId = model.ProductId,
                StoreName = model.StoreName,
                ProductName = model.ProductName,
                Price = model.Price,
                Count = model.Count,
                unitOfmass = model.unitOfmass,
                Description = model.Description,
                Mass = model.Mass,
                AddDate = model.AddDate
            };
            _storeProductService.Add(storeProduct);
            return RedirectToAction("Index", "Seller");
        }

        [HttpGet]
        public IActionResult ShowStoreProduct(int? storeId)
        {
            var data = _storeProductService.GetProductByStoreId(storeId);
            return View(data);

        }

        [HttpPost]
        public IActionResult ShowStoreProduct()
        {
            var data = _storeProductService.GetAll();
            return View(data);
        }

        [HttpGet]
        public IActionResult EditAllStore(int id)
        {
            var data = _storeProductService.GetById(id);
            return View(data);  
        }

        [HttpPost]
        public IActionResult EditAllStore(StoreProductViewModel model)
        {
            _storeProductService.Update(model);
            return RedirectToAction("ShowStoreProduct", new { StoreId = model.StoreId });
        }

        [HttpPost]
        public IActionResult DeleteStoreProduct(StoreProductViewModel model)
        {   
            var data = _storeProductService.GetById(model.Id);
            _storeProductService.Delete(data);
            return RedirectToAction("ShowStoreProduct", new { Storeid = model.StoreId });
        }

    }

}


