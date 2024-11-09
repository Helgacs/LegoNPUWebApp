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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadImages(int page = 1, int pageSize = 10)
        {
            var images = await _imageService.GetImagesAsync(page, pageSize);
            return PartialView("_ImageListPartial", images);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile imageFile, string description)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var imageUrl = await SaveFileToStorage(imageFile);

                await _imageService.AddImageAsync(imageUrl, description, Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));

            }
            return RedirectToAction("Index", "Home");
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
