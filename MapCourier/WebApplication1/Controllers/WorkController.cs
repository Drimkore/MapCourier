using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class WorkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Pickup()
        {
            return View();
        }
        public IActionResult Deliver()
        {
            return View();
        }
    }
}
