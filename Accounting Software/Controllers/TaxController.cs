using Accounting_Software.Repository_Interfaces;
using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    [Authorize]
    public class TaxController : Controller
    {
        private readonly ITaxService _taxService;
        private readonly IUserRepository _userRepository;

        public TaxController(ITaxService taxService, IUserRepository userRepository)
        {
            _taxService = taxService;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            var user = _userRepository.GetAll().FirstOrDefault();
            if (user == null)
                return RedirectToAction("Index", "Dashboard");

            var model = _taxService.GetOverview(user.Id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(TaxRuleViewModel model)
        {
            var user = _userRepository.GetAll().FirstOrDefault();
            if (user == null)
                return RedirectToAction("Index", "Dashboard");

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please enter a valid name and rate (0–100%).";
                return RedirectToAction("Index");
            }

            model.UserId = user.Id;
            _taxService.Add(model);
            TempData["Success"] = $"Tax '{model.Name}' added.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(TaxRuleViewModel model)
        {
            var user = _userRepository.GetAll().FirstOrDefault();
            if (user == null)
                return RedirectToAction("Index", "Dashboard");

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please enter a valid name and rate (0–100%).";
                return RedirectToAction("Index");
            }

            model.UserId = user.Id;
            _taxService.Update(model);
            TempData["Success"] = $"Tax '{model.Name}' updated.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _taxService.Delete(id);
            TempData["Success"] = "Tax removed.";
            return RedirectToAction("Index");
        }
    }
}
