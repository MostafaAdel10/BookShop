using BookShop.Core.Bases;
using BookShop.Core.Features.Discount.Commands.Validations;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookShop.Core.Features.Books.Commands.Models
{
    public record AddImagesCommand : IRequest<Response<string>>
    {
        public int BookId { get; set; }

        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".svg", ".webp" })]
        public ICollection<IFormFile>? Images { get; set; }
    }
}
