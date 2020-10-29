using Microsoft.AspNetCore.Mvc;

namespace Rides.API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
