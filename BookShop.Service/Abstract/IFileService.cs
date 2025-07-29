using Microsoft.AspNetCore.Http;

namespace BookShop.Service.Abstract
{
    public interface IFileService
    {
        Task<List<string>> UploadImagesAsync(IEnumerable<IFormFile> images, string folder);
        Task<string?> UploadImageAsync(IFormFile image, string folder);
        bool DeleteImage(string? imageUrl);
        Task<string?> UpdateImageAsync(string? oldImageUrl, IFormFile newImage, string folder);

    }
}
