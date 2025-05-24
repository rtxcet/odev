using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odev.Models;

namespace Odev.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        // Mesaj listesini göster
        public IActionResult Index()
        {
            var messages = _context.Contact.OrderByDescending(c => c.Id).ToList();
            return View(messages);
        }

        // Silme işlemi
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var message = _context.Contact.Find(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Contact.Remove(message);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Mesaj başarıyla silindi.";
            return RedirectToAction("Index");
        }
    }
}
