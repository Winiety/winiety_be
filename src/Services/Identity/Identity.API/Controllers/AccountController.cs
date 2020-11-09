using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.API.ViewModels;
using Identity.Core.Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var vm = new LoginViewModel
            {
                ReturnUrl = returnUrl,
            };

            ViewData["ReturnUrl"] = returnUrl;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ViewData["ReturnUrl"] = model.ReturnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return Redirect("~/");
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password.");

            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            var vm = new RegisterViewModel
            {
                ReturnUrl = returnUrl,
            };

            ViewData["ReturnUrl"] = returnUrl;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ViewData["ReturnUrl"] = model.ReturnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser(model.Email);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Errors.Count() > 0)
            {
                AddErrors(result);
                return View(model);
            }

            await _signInManager.SignInAsync(user, false);

            if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return Redirect("~/");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
