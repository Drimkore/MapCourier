using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MapCourier.Models;
using MapCourier.Data;
using System.Security.Claims;

namespace MapCourier.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MapContext _context;


    public HomeController(ILogger<HomeController> logger, MapContext context)
    {
        _context = context;
        _logger = logger;
    }
    public IActionResult Index(string action)
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var delivery = (_context.Delivery.Where(d => d.UserID == user));
            var acceptance = delivery.Where(d => d.Order.status == "acceptance");
            if (acceptance != null && acceptance.Any())
            {

                foreach (var d in delivery)
                {
                    _context.Delivery.Remove(d);
                }
                foreach (var a in acceptance)
                {
                    var order = _context.Order.FirstOrDefault(o => o.OrderID == a.OrderID);
                    order.status = "waiting";
                    _context.Order.Update(order);
                }
                _context.SaveChangesAsync();


            }
            if (action == "work")
                return Redirect("../Work/Index");
            //if (User.Identity.IsAuthenticated) { return View(); }  РУКАМИ НЕ ТРОГАТЬ
            //else { return NotFound();
            //}
            return View();
        }
        else
        {
            return View();
        }
        
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
