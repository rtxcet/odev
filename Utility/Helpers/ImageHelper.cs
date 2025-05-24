namespace Odev.Utility.Helpers
{
    public class ImageHelper
    {
        public static string UploadImage(IFormFile img, string folderName, IWebHostEnvironment env)
        {
            if (img == null) return null;

            // assets/uploads/{folderName} dizinine kaydedecek şekilde klasör yolu oluşturuyoruz
            var folderPath = Path.Combine(env.WebRootPath, "assets", "uploads", folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Benzersiz dosya adı oluştur
            var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            // Dosyayı kaydet
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                img.CopyTo(stream);
            }

            // Veritabanına kaydedilecek yol
            return $"/assets/uploads/{folderName}/{fileName}";
        }

        public static void DeleteImage(string fileName, string folderName, IWebHostEnvironment env)
        {
            if (string.IsNullOrEmpty(fileName)) return;

            var fullPath = Path.Combine(env.WebRootPath, "assets", "uploads", folderName, fileName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
