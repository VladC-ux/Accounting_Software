﻿using Microsoft.AspNetCore.Mvc;

namespace Accounting_Software.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
