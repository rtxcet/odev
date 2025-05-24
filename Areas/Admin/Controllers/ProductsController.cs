using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Odev.Models;
using Odev.Utility.Helpers;

namespace Odev.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Products product, IFormFile img)
        {
            if (img != null)
            {
                product.Image = ImageHelper.UploadImage(img, "products", _env);
            }

            // Veritabanına ekle
            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }



        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Products product, IFormFile img)
        {
            var oldProduct = _context.Products.Find(product.Id);
            if (oldProduct == null)
            {
                return NotFound();
            }

            if (img != null && img.Length > 0)
            {
                // Eski resmi sil
                if (!string.IsNullOrEmpty(oldProduct.Image))
                {
                    var fileName = Path.GetFileName(oldProduct.Image); // sadece dosya adı al
                    ImageHelper.DeleteImage(fileName, "products", _env);
                }

                // Yeni resmi yükle
                oldProduct.Image = ImageHelper.UploadImage(img, "products", _env);
            }


            // Diğer alanları güncelle
            oldProduct.Name = product.Name;
            oldProduct.Description = product.Description;
            oldProduct.Price = product.Price;

            _context.SaveChanges();

            TempData["info"] = "Ürün başarıyla güncellendi";
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.Image))
                {
                    var fileName = Path.GetFileName(product.Image); // sadece dosya adı al
                    ImageHelper.DeleteImage(fileName, "products", _env);
                }


                _context.Products.Remove(product);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}
