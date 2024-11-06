using LegoNPUWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LegoNPUWebApp.Controllers
{
    public class ImageController : Controller
    {
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _environment;

        public ImageController(ImageService imageService, IWebHostEnvironment environment)
        {
            _imageService = imageService;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> LoadImages(int page = 1, int pageSize = 10)
        {
            var images = await _imageService.GetImagesAsync(page, pageSize);
            return PartialView("_ImageListPartial", images);
        }

        public IActionResult Upload()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, string description)
        {
            if (file != null && file.Length > 0)
            {
                var imageUrl = await SaveFileToStorage(file);

                await _imageService.AddImageAsync(imageUrl, description, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }
            return RedirectToAction("Index");
        }

        private async Task<string> SaveFileToStorage(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images/uploads");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/images/uploads/{uniqueFileName}";
        }
    }
}
