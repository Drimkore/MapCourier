using Microsoft.AspNetCore.Mvc;
using MapCourier.Data;
using MapCourier.Models;
using MapCourier.Controllers;

namespace MapCourier.Controllers
{
    public class WorkController : Controller
    {
        private List<Mark> Marks;
        private int Iteration;
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string latitude, string longitude) 
        {
            Marks = FinalResult.GetResultPath(latitude, longitude);
            return Redirect("../Work/Pickup");
        }
        public IActionResult Pickup()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult Pickup()
        //{

        //}
        public IActionResult Deliver()
        {
            return View();
        }
    }
}
