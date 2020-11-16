﻿using Microsoft.AspNetCore.Mvc;

namespace Profile.API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
