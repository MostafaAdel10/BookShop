using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.CartItem.Commands.Models
{
    public class AddCartItemCommand : IRequest<Response<string>>
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}