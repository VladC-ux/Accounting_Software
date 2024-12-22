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
            if (ModelState.IsValid)
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
                return RedirectToAction("ShowSellerProduct","Product");
            }

            return View(model); 
        }



















    }
}
