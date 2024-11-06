using Accounting_Software.Data.Entites;
using Accounting_Software.Service;
using Accounting_Software.Service_Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    public class StoreController : Controller
    {
        private readonly IStoreService _storeService;
        private readonly IStoreProductService _storeProductService;
     

        public StoreController(IStoreService storeService, IStoreProductService storeproduct)
        {
            _storeService = storeService;
            _storeProductService = storeproduct;
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
        public IActionResult ProductToStore(int productId, int storeId)
        {
            _storeProductService.AddProductToStore(productId, storeId);
            return View();
        }

        public IActionResult ShowShops(int productId,int storeId)
        {
            var data = _storeProductService.GetStoreProduct(productId, storeId);
            return View(data);
        }
    }
}
