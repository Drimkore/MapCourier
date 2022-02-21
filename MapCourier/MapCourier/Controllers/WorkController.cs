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
            if (!_context.Storage.Any())
                return Redirect("../Storages");
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var delivery = _context.Delivery.Where(d => d.UserID == user);
            if (delivery.Any())
            {
                var orderID = delivery.First().OrderID;
                var order = _context.Order.FirstOrDefault(o => o.OrderID == orderID);
                if (order == null)
                    return Redirect("../Work/Pickup");
                if (order.status == "busy")
                    return Redirect("../Work/Deliver");
            }
            if (!_context.Order.Where(o => o.status == "waiting").Any())
            {
                return Redirect("../Work/Finish");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Index(string latitude, string longitude) 
        {
            if (!User.Identity.IsAuthenticated){
                return NotFound();
            }
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var delivers = _context.Delivery.Where(d => d.UserID == user);
            var orders = new List<Order>();
            if (delivers.Any())
            {
                foreach (var d in delivers)
                {
                    if (d.OrderID == null)
                        continue;
                    orders.Add(_context.Order.FirstOrDefault(o => o.OrderID == d.OrderID));
                }
                return View(orders);
            }
            var marks = FinalResult.GetResultPath(latitude, longitude);

            foreach (var m in marks)
            {
                Delivery d = new();
                d.UserID = user;
                if (m.Status == "storage")
                {
                    d.StorageID = m.ID;
                    _context.Delivery.Add(d);
                }
                if(m.Status == "acceptance")
                {
                    d.OrderID = m.ID;
                    
                    _context.Delivery.Add(d);
                }
            }
            _context.SaveChanges();
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
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var delivery = _context.Delivery.Where(d => d.UserID == user);
            foreach(var d in delivery)
            {
                if (d.OrderID == null)
                    continue;
                var order = _context.Order.FirstOrDefault(o => o.OrderID == d.OrderID);
                order.status = "busy";
                _context.Order.Update(order);
            }
            _context.SaveChanges();
            return Redirect("../Work/Pickup");
        }

        public IActionResult Pickup(string action)
        {
            if (!User.Identity.IsAuthenticated){
                return NotFound();
            }
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var delivery = _context.Delivery.FirstOrDefault(d => d.UserID == user);           
            var storage = _context.Storage.FirstOrDefault(s => s.StorageID == delivery.StorageID);          
            if (action == "redirect")
            {
                _context.Delivery.Remove(delivery);
                _context.SaveChanges();
                delivery = _context.Delivery.FirstOrDefault(d => d.UserID == user);
                if (delivery == null)
                    return Redirect("../Work/Index");
                return Redirect("../Work/Deliver");
            }
            return View(storage);
        }

        public IActionResult Deliver(string action)
        {
            if (!User.Identity.IsAuthenticated){
                return NotFound();
            }
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var delivery = _context.Delivery.FirstOrDefault(d => d.UserID == user);   
            var order = _context.Order.FirstOrDefault(s => s.OrderID == delivery.OrderID);
            if (action == "redirect")
            {
                order.status = "finished";
                _context.Order.Update(order);
                _context.Delivery.Remove(delivery);
                _context.SaveChanges();
                delivery = _context.Delivery.FirstOrDefault(d => d.UserID == user);
                order = _context.Order.FirstOrDefault(s => s.OrderID == delivery.OrderID);
            }
            if (delivery.OrderID == null)
            {
                return Redirect("../Work/Pickup");
            }
            return View(order);
        }
        public IActionResult Finish()
        { 
        return View();
        }
    }
}