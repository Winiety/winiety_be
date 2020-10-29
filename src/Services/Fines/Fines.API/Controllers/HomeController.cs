using Microsoft.AspNetCore.Mvc;

namespace Fines.API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
