using Accounting_Software.Data.Entites;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace Accounting_Software.Controllers
{
    public class StoreController : Controller
    {
        private readonly IStoreService _storeService;
        private readonly IStoreProductService _storeProductService;
        private readonly IProductService _productService;
        private readonly IStoreProductRepository _storeProductRepository;
     

        public StoreController(IStoreService storeService, IStoreProductService storeproduct, IStoreProductRepository storeProductRepository,IProductService productService)
        {
            _storeService = storeService;
            _storeProductService = storeproduct;
            _storeProductRepository = storeProductRepository;
            _productService = productService;
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
        //{
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
                return NotFound("Магазин не найден.");
            }
            
            var product = _productService.GetById(productId);
            if (product == null)
            {
                return NotFound("Продукт не найден.");
            }
            
            var viewModel = new StoreProductViewModel
            {
                
                StoreId = storeId,
                ProductId = productId,
                StoreName = store.StoreName,
                ProductName = product.Name,
                Price = product.Price, 
                
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddProductToStore(StoreProductViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                var storeProduct = new StoreProductViewModel
                {
                    StoreId = model.StoreId,
                    ProductId = model.ProductId,
                    Price = model.Price,
                    Count = model.Count
                };

                _storeProductService.Add(storeProduct); 

                return RedirectToAction("ShowSellerProduct","Product");
            }

            return View(model); 
        }



















    }
}
