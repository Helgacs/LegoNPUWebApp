using LegoNPU.Data;
using LegoNPU.Models;
using Microsoft.EntityFrameworkCore;

namespace LegoNPUWebApp.Services
{
    public class ImageService
    {
        private readonly AppDBContext _context;

        public ImageService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Image>> GetImagesAsync(int page, int pageSize)
        {
            return await _context.Images
                .Include(i => i.User)
                .OrderByDescending(i => i.UploadedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Image>> GetUserImagesAsync(Guid userId)
        {
            return await _context.Images
                .Where(img => img.UserId == userId)
                .OrderByDescending(img => img.UploadedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Image>> SearchImagesAsync(string keyword)
        {
            return await _context.Images
                .Where(i => i.Description.Contains(keyword))
                .ToListAsync();
        }

        public async Task AddImageAsync(string url, string description, Guid userId, string title = "title")
        {
            User user = _context.Users
                .FirstOrDefault(u => u.Id == userId);

            var image = new Image
            {
                Url = url,
                Description = description,
                UserId = userId,
                UploadedAt = DateTime.UtcNow,
                User = user,
                Title = title
            };

            _context.Images.Add(image);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteImageAsync(Guid imageId, Guid userId)
        {
            Image image = _context.Images
                .FirstOrDefault(u => u.Id == imageId && u.UserId == userId);

            if (image == null) 
            {
                return false;
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.Url.TrimStart('/'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task RateImageAsync(Guid imageId, int score, Guid userId)
        {
            var rating = new Rating { ImageId = imageId, Score = score, UserId = userId };
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
        }

        public async Task<double> GetAverageRatingAsync(Guid imageId)
        {
            return await _context.Ratings
                .Where(r => r.ImageId == imageId)
                .AverageAsync(r => (double)r.Score);
        }
    }
}
