using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;

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
