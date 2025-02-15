using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.CartItem.Commands.Models
{
    public class DeleteCartItemCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteCartItemCommand(int id)
        {
            Id = id;
        }
    }
}
