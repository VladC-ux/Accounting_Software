using Accounting_Software.Data.Entites;
using Accounting_Software.Service_Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    public class StoreController : Controller
    {
        private readonly IStoreService _storeService;
        private readonly IStoreProductService _storeProduct;
      
        public StoreController(IStoreService storeService, IStoreProductService storeproduct)
        {
            _storeService = storeService;
            _storeProduct = storeproduct;
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
        public async Task<IActionResult> ProductToStore(int productId)
        {
            await _storeProduct.AddProductToStoreAsync(productId);
            return View();
        }
    }
}
