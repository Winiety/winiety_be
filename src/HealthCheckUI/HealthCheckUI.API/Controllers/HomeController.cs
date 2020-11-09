using Microsoft.AspNetCore.Mvc;

namespace HealthCheckUI.API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("~/hc-ui");
        }
    }
}
