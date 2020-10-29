﻿using Microsoft.AspNetCore.Mvc;

namespace Payment.API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
