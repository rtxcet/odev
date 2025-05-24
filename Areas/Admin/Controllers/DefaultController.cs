using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odev.Models;
using Odev.Utility.Helpers;

namespace Odev.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DefaultController : Controller
    {
     
        public IActionResult Index()
        {
            return View();
        }

     
    }
}
