using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            //var test = FinalResult.GetResultPath("50", "51");
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            if (username.Any() || password.Any())
            {
                using (var db = new MapContext())
                {
                    var user = db.User.Find(username);
                    if (user != null)
                    {
                        if (user.Password == password)
                        {
                            return View();
                        }
                    }
                }
            }
            ViewBag.Message = "Не верно введены поля логина или пароля";
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
}