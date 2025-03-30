using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    public class SellerController : Controller
    {
        private readonly ISellerService _sellerService;

        public SellerController(ISellerService userinterface)
        {
            _sellerService = userinterface;
        }


        public IActionResult Index()
        {
          
            ViewBag.Sellers = _sellerService.GetAll();
            var list = _sellerService.GetAll();
            return View(list);
            
        }

        public IActionResult Add(SellerViewModel model)
        {
            if (model.Name == null)
            {
                ModelState.AddModelError(nameof(SellerViewModel.Name), "Please select Sellers");
            }
            if (!ModelState.IsValid)
            {
                GetProductDropdownData();
                return View(model);
            }
            _sellerService.Add(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var querryedit = _sellerService.GetById(id);
            return View(querryedit);
        }
        [HttpPost]
        public IActionResult Edit(SellerViewModel model)
        {
            _sellerService.Update(model);
            return RedirectToAction("Index");
        }
      
        public IActionResult Delete(int id)
        {
            _sellerService.Delete(id);
            return RedirectToAction("Index");
        }      
        private void GetProductDropdownData()
        {
            ViewBag.Sellers = _sellerService.GetAll();
        }
    }
}
