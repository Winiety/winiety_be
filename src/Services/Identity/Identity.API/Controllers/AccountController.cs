using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Identity.API.Models;
using Identity.API.Options;
using Identity.API.ViewModels;
using Identity.Core.Data.Model;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Identity.API.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ReCaptchaOptions _reCaptchaOptions;
        private readonly IIdentityServerInteractionService _interaction;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IOptions<ReCaptchaOptions> reCaptchaOptions,
            IHttpClientFactory clientFactory,
            IIdentityServerInteractionService interaction)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _clientFactory = clientFactory;
            _reCaptchaOptions = reCaptchaOptions.Value;
            _interaction = interaction;
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
            
            var isReCaptchaValid = await CheckReCaptcha(model.ReCaptchaToken);

            if (!isReCaptchaValid)
            {
                ModelState.AddModelError(string.Empty, "ReCaptcha failed. You are a robot...");

                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");

                return View(model);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, true);

            if (result.Succeeded)
            {
                var tokenLifetime = 120;

                var props = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(tokenLifetime),
                    AllowRefresh = true,
                    RedirectUri = model.ReturnUrl
                };

                if (model.RememberMe)
                {
                    var permanentTokenLifetime = 365;

                    props.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(permanentTokenLifetime);
                    props.IsPersistent = true;
                };

                await _signInManager.SignInAsync(user, props);

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

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

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

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return await Logout(new LogoutViewModel { LogoutId = logoutId });
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                return await Logout(new LogoutViewModel { LogoutId = logoutId });
            }

            var vm = new LogoutViewModel
            {
                LogoutId = logoutId
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutViewModel model)
        {
            await HttpContext.SignOutAsync();

            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            var logout = await _interaction.GetLogoutContextAsync(model.LogoutId);

            return Redirect(logout?.PostLogoutRedirectUri);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private async Task<bool> CheckReCaptcha(string recaptchaToken)
        {
            var httpClient = _clientFactory.CreateClient();
            var response = await httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_reCaptchaOptions.ReCaptchaSecretKey}&response={recaptchaToken}");
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var reCaptchaResponse = JsonSerializer.Deserialize<ReCaptchaResponse>(responseContent);

            return reCaptchaResponse.success && reCaptchaResponse.score > 0.5;
        }
    }
}
