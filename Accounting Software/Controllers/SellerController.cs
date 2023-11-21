using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    public class SellerController : Controller
    {
        private readonly ISellerServiceInterface _sellerService;

        public SellerController(ISellerServiceInterface userinterface)
        {
            _sellerService = userinterface;
        }


        public IActionResult Index()
        {
            var list = _sellerService.GetAll();
            return View(list);
            
        }

        public IActionResult Add(AddEditViewModel model)
        {
            if (model.Name == null)
            {
                ModelState.AddModelError(nameof(AddEditViewModel.Name), "Please select Sellers");
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
      

        public IActionResult Delete(SellerViewModel model)
        {
            _sellerService.Delete(model);
            return RedirectToAction("Index");
        }

        private void GetProductDropdownData()
        {
            ViewBag.Sellers = _sellerService.GetAll();

        }

    }
}
