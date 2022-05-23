using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MapCourier.Models;
using MapCourier.Data;
using System.Security.Claims;

namespace MapCourier.Controllers;

public class OperatorController: Controller
{
    private readonly MapContext _context;

    public OperatorController(MapContext context)
    {
        _context = context;
    }

    public IActionResult Index(string action, int? deliveryID)
    {
        int counter = 0;
        var delivery = _context.Delivery;
        
        var storage = _context.Storage;
        if (action == "delete")
        {
            var dataToDelete = delivery.Where(d => d.DeliveryID == deliveryID);
            var busy = dataToDelete.Where(d => d.Order.status == "busy");
            foreach (var item in dataToDelete)
            {
                _context.Delivery.Remove(item);
            }
            foreach (var a in busy)
            {
                var order = _context.Order.FirstOrDefault(o => o.OrderID == a.OrderID);
                order.status = "waiting";
                _context.Order.Update(order);
            }
                _context.SaveChangesAsync();
        }
        ViewBag.Delivery = _context.Delivery;
        ViewBag.Order = _context.Order;
        ViewBag.Storage = _context.Storage;
        foreach (var item in delivery)
            if (item.OrderID is not null)
                counter++;
        ViewBag.Counter = counter;
        
        return View();    
    }
}