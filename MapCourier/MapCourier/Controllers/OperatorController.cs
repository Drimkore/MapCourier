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

    public IActionResult Index()
    {
        var delivery = _context.Delivery;
        var order = _context.Order;
        var storage = _context.Storage;

        ViewBag.Delivery = _context.Delivery;
        ViewBag.Order = _context.Order;
        ViewBag.Storage = _context.Storage;


        return View();    
    }

    //[HttpPost]
    /*public async IActionResult Index()
    {
        return View();
    }*/

  
}