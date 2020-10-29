using Microsoft.AspNetCore.Mvc;

namespace Statistics.API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
