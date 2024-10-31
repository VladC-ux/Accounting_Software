using Accounting_Software.Service_Interfaces;
using Accounting_Software.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    public class WareHouseController : Controller
    {
        private readonly IWareHouseService _warehouse;
        private readonly IProductService _productService;
        public WareHouseController(IWareHouseService warehouse, IProductService productService)
        {
            _warehouse = warehouse;
            _productService = productService;
        }

        //[HttpGet]
        //public IActionResult ToWareHouse(int id)
        //{
        //    _productService.GetById(id);
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult ToWareHouse(ProductViewModel model)
        //{
        //    _productService.AddToWareHouse(model);
        //    return RedirectToAction("Seller","Index");
        //}
    }
}
