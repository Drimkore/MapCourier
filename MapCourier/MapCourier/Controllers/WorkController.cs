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
            
            if (!_context.Order.Where(o => o.status == "waiting").Any())
            {
                return Redirect("../Work/Finish");
            }
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (_context.Delivery.Where(d => d.UserID == user).Any())
            {
                return Redirect("../Work/Pickup");
            }
                return View();
        }
        [HttpPost]
        public IActionResult Index(string latitude, string longitude) 
        {
            if (!User.Identity.IsAuthenticated){
                return NotFound();
            }
            var marks = FinalResult.GetResultPath(latitude, longitude);
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int? storage = 0;
            foreach (var m in marks)
            {
                Delivery d = new();
                d.UserID = user;
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
                UserID = user,
                StorageID = marks.Last().ID
            });
            _context.SaveChanges();
            var orders = new List<Order>();
            foreach(var m in marks)
            {
                if (m.Status == "storage")
                    continue;
                orders.Add(_context.Order.FirstOrDefault(o => o.OrderID == m.ID));
            }
            return View(orders);
            
        }
        [HttpPost]
        public IActionResult RedirectToPickup()
        {
            return Redirect("../Work/Pickup");
        }

        public IActionResult Pickup(string action)
        {
            if (!User.Identity.IsAuthenticated){
                return NotFound();
            }

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
            if (!User.Identity.IsAuthenticated){
                return NotFound();
            }
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var delivery = _context.Delivery.FirstOrDefault(d => d.UserID == user);
            if (delivery == null)
            {
                return Redirect("../Work/Finish");
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
        public IActionResult Finish()
        { 
        return View();
        }
    }
}