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
            int? storage = 0;
            foreach (var m in marks)
            {
                Delivery d = new();
                d.UserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                d.OrderID = null;
                if (m.Status == "storage")
                {
                    storage = m.ID;
                }
                if(m.Status == "busy")
                {
                    d.OrderID = m.ID;
                    d.StorageID = storage;
                    _context.Delivery.Add(d);
                }
            }

            _context.Delivery.Add(new Delivery() 
            {
                UserID = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                StorageID = marks.Last().ID
            });
            _context.SaveChanges();
            return View();
            
        }
        [HttpPost]
        public IActionResult RedirectToPickup()
        {
            return Redirect("../Work/Pickup");
        }

        public IActionResult Pickup(string action)
        {

            Storage storage = new Storage();
            if (action == "redirect")
                return Redirect("../Work/Deliver");
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var delivery = _context.Delivery.FirstOrDefault(d => d.UserID == user);
            
            if(delivery.OrderID == null)
            {
                storage = _context.Storage.FirstOrDefault(s => s.StorageID == delivery.StorageID);
                _context.Delivery.Remove(delivery);
                _context.SaveChanges();
                return View(storage);
            }
            storage = _context.Storage.FirstOrDefault(s => s.StorageID == delivery.StorageID);
            return View(storage);
        }

        public IActionResult Deliver()
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var delivery = _context.Delivery.FirstOrDefault(d => d.UserID == user);
            if (delivery == null)
            {
                return NotFound();
            }
            if (delivery.OrderID == null)
            {
                return Redirect("../Work/Pickup");
            }
            var order = _context.Order.FirstOrDefault(s => s.OrderID == delivery.OrderID);
            order.status = "finished";
            _context.Order.Update(order);
            _context.Delivery.Remove(delivery);
            _context.SaveChanges();
            return View(order);
        }
    }
}