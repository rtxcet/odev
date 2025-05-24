using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Odev.Models;

namespace Odev.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

     

        public IActionResult Index()
        {
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
