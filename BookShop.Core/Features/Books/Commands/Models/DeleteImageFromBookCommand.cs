using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Books.Commands.Models
{
    public class DeleteImageFromBookCommand : IRequest<Response<string>>
    {
        public int BookId { get; set; }
        public string ImageUrl { get; set; }
    }
}
