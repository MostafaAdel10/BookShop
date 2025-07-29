using BookShop.Service.Abstract;
using Microsoft.AspNetCore.Http;

namespace BookShop.Service.Implementations
{
    public class FileService : IFileService
    {
        private readonly string _rootPath = "wwwroot";
        public async Task<string?> UploadImageAsync(IFormFile image, string folder)
        {
            try
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                var filePath = Path.Combine(_rootPath, "images", folder, fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                return $"/images/{folder}/{fileName}";
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<string>> UploadImagesAsync(IEnumerable<IFormFile> images, string folder)
        {
            List<string> imageUrls = new();

            string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", folder);
            Directory.CreateDirectory(uploadFolder);

            foreach (var imageFile in images)
            {
                try
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                    string filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    imageUrls.Add($"/images/{folder}/{fileName}");
                }
                catch
                {
                    throw new Exception("Failed to upload image.");
                }
            }

            return imageUrls;
        }

        public bool DeleteImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return false;

            try
            {
                var filePath = Path.Combine(_rootPath, imageUrl.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string?> UpdateImageAsync(string? oldImageUrl, IFormFile newImage, string folder)
        {
            DeleteImage(oldImageUrl);
            return await UploadImageAsync(newImage, folder);
        }
    }

}