using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Books.Commands.Models
{
    public class DeleteBookCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteBookCommand(int id)
        {
            Id = id;
        }
    }
}
