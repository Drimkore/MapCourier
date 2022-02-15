using Microsoft.AspNetCore.Mvc;
using MapCourier.Data;
using MapCourier.Models;
using MapCourier.Controllers;

namespace MapCourier.Controllers
{
    public class WorkController : Controller
    {
        private readonly MapContext _context;

        public WorkController(MapContext context)
        {
            _context = context;
        }
        private OrdersMarks Marks;
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string latitude, string longitude) 
        {
            Marks = new(FinalResult.GetResultPath(latitude, longitude));
            return View();
            
        }
        [HttpPost]
        public IActionResult RedirectToPickup()
        {
            return Redirect("../Work/Pickup");
        }

        public IActionResult Pickup()
        {
            return View(Marks);
        }
        //[HttpPost]
        //public IActionResult Pickup()
        //{

        //}
        public IActionResult Deliver()
        {

            var mark = Marks.GetMark();
            if (mark.Status == "busy")
            {
                mark.Status = "finished";
                var order = _context.Order.FirstOrDefault(o => o.OrderID == mark.ID);
                order.status = "finished";
                _context.SaveChangesAsync();
            }
            Marks.Next();
            return View(Marks);
        }
    }
}
