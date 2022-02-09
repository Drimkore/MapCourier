using Microsoft.AspNetCore.Mvc;
using MapCourier.Data;

namespace MapCourier.Controllers
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
