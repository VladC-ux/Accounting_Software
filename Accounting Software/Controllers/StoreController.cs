using Accounting_Software.Data.Entities;
using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Accounting_Software.Controllers
{
    [Authorize]
    public class StoreController : Controller
    {
        private readonly IStoreService _storeService;
        private readonly IStoreProductService _storeProductService;
        private readonly IProductService _productService;
        private readonly IStoreProductRepository _storeProductRepository;
        private readonly ISellerService _sellerService;
        private readonly IUserRepository _userRepository;


        public StoreController(IStoreService storeService, IStoreProductService storeproduct, IStoreProductRepository storeProductRepository, IProductService productService, ISellerService sellerService,IUserRepository userRepository)
        {
            _storeService = storeService;
            _storeProductService = storeproduct;
            _storeProductRepository = storeProductRepository;
            _productService = productService;
            _sellerService = sellerService;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
           
            var stores = _storeService.GetAll();
            return View(stores);
        }

        public IActionResult Sale(StoreProductViewModel model,int userid)
        {
            try
            {
                _storeProductService.GetBalanceSale(model.Id,userid);
                TempData["SuccessMessage"] = "Sale completed successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("ShowStoreProduct", new { Storeid = model.StoreId });
        }

        [HttpPost]
        public IActionResult DeleteStoreProduct(StoreProductViewModel model)
        {
            var data = _storeProductService.GetById(model.Id);
            _storeProductService.Delete(data);
            return RedirectToAction("ShowStoreProduct", new { Storeid = model.StoreId});
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
        public IActionResult ProductToStore(int productId, int storeId)
        {

            _storeProductService.AddProductToStore(productId, storeId);
            return View();
        }

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
            try
            {
                if (model.ProductId <= 0)
                    throw new InvalidOperationException("Please select a product.");

                if (string.IsNullOrEmpty(model.ProductName) || string.IsNullOrEmpty(model.StoreName))
                {
                    var prod = _productService.GetById(model.ProductId);
                    var st = _storeService.GetById(model.StoreId);
                    model.ProductName = prod?.Name ?? model.ProductName;
                    model.StoreName = st?.StoreName ?? model.StoreName;
                }

                var storeProduct = new StoreProductViewModel
                {
                    Id = 0,
                    StoreId = model.StoreId,
                    ProductId = model.ProductId,
                    StoreName = model.StoreName,
                    ProductName = model.ProductName,
                    Price = model.Price,
                    Count = model.Count,
                    unitOfmass = model.unitOfmass,
                    Description = model.Description,
                    Mass = model.Mass,
                    AddDate = DateTime.Now
                };
                _storeProductService.Add(storeProduct);
                TempData["SuccessMessage"] = $"\"{storeProduct.ProductName}\" added to {storeProduct.StoreName}.";
                return RedirectToAction("ShowStoreProduct", new { storeId = model.StoreId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("ShowStoreProduct", new { storeId = model.StoreId });
            }
        }

        [HttpGet]
        public IActionResult PickProductForStore(int storeId)
        {
            var store = _storeService.GetById(storeId);
            if (store == null)
            {
                return NotFound("Store not found.");
            }

            ViewBag.StoreId = store.Id;
            ViewBag.StoreName = store.StoreName;
            var products = _productService.GetAll();
            return View(products);
        }

        [HttpGet]
        public IActionResult ShowStoreProduct(int? storeId)
        {
            ViewBag.Users = _userRepository.GetAll();
            ViewBag.AllProducts = _productService.GetAll();
            if (storeId.HasValue)
            {
                var store = _storeService.GetById(storeId.Value);
                ViewBag.StoreId = storeId.Value;
                ViewBag.StoreName = store?.StoreName;
            }
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
    }

}


