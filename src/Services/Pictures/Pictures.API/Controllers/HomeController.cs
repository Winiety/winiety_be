using Microsoft.AspNetCore.Mvc;

namespace Pictures.API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
