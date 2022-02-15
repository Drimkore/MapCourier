using Microsoft.AspNetCore.Mvc;
using MapCourier.Data;
using MapCourier.Models;
using MapCourier.Controllers;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MapCourier.Controllers
{
    public class WorkController : Controller
    {
        private readonly MapContext _context;

        public WorkController(MapContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string latitude, string longitude) 
        {
            var marks = FinalResult.GetResultPath(latitude, longitude);
            Delivery delivery = new();
            delivery.UserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            foreach (var m in marks)
            {
                delivery.OrderID = null;
                if (m.Status == "storage")
                {
                    delivery.StorageID = m.ID;
                    delivery.Storage = _context.Storage.FirstOrDefault(s => s.StorageID == m.ID);
                }
                if(m.Status == "busy")
                {
                    delivery.OrderID = m.ID;
                    delivery.Order = _context.Order.FirstOrDefault(s => s.OrderID == m.ID);
                    _context.Delivery.Add(delivery);
                }
            }
            _context.Delivery.Add(delivery);
            _context.SaveChanges();
            return View();
            
        }
        [HttpPost]
        public IActionResult RedirectToPickup()
        {
            return Redirect("../Work/Pickup");
        }

        public IActionResult Pickup()
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var delivery = _context.Delivery.FirstOrDefault(d => d.UserID == user);
            var storage = _context.Storage.FirstOrDefault(s => s.StorageID == delivery.StorageID);
            return View(storage);
        }
        //[HttpPost]
        //public IActionResult Pickup()
        //{

        //}
        public IActionResult Deliver()
        {
            //var mark = Marks.GetMark();
            //if (mark.Status == "busy")
            //{
            //    mark.Status = "finished";
            //    var order = _context.Order.FirstOrDefault(o => o.OrderID == mark.ID);
            //    order.status = "finished";
            //    _context.SaveChangesAsync();
            //}
            //Marks.Next();
            return View();
        }
    }
}
