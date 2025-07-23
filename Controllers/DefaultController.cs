using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odev.Models;

namespace Odev.Controllers
{
    public class DefaultController : Controller
    {
        private readonly AppDbContext _context;

        public DefaultController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();  // Veritabanından ürünleri çekiyoruz
            return View(products);                      // View'e model olarak gönderiyoruz
        }



        public IActionResult About()
        {
            return View();
        }
     
        public IActionResult ProductDetail(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(Contact model)
        {
            if (ModelState.IsValid)
            {
                _context.Contact.Add(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Mesajınız başarıyla gönderildi!";
                return RedirectToAction("Contact");
            }
            return View(model);
        }
    }

}
