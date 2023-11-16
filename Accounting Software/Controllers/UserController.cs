using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServiceInterface _userService;

        public UserController(IUserServiceInterface userinterface)
        {
            _userService = userinterface;
        }


        public IActionResult Index()
        {
            var list = _userService.GetAll();
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
            _userService.Add(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var querryedit = _userService.GetById(id);
            return View(querryedit);
        }
        [HttpPost]
        public IActionResult Edit(UserViewModel model)
        {
            _userService.Update(model);
            return RedirectToAction("Index");
        }
      

        public IActionResult Delete(UserViewModel model)
        {
            _userService.Delete(model);
            return RedirectToAction("Index");
        }

        private void GetProductDropdownData()
        {
            ViewBag.Sellers = _userService.GetAll();

        }

    }
}
