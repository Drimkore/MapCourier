using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MapCourier.Models;
using MapCourier.Controllers;

namespace MapCourier.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    public IActionResult Index(string action)
    {
        if(action == "work")
            return Redirect("../Work/Index");
        //if (User.Identity.IsAuthenticated) { return View(); }  РУКАМИ НЕ ТРОГАТЬ
        //else { return NotFound();
        //}
        return View();
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
