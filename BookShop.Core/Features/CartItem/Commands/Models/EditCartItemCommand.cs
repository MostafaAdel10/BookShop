using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.CartItem.Commands.Models
{
    public class EditCartItemCommand : IRequest<Response<CartItemCommand>>
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public int ShoppingCartId { get; set; }
    }
}
