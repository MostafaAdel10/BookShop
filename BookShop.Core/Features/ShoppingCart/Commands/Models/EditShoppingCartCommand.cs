using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.ShoppingCart.Commands.Models
{
    public class EditShoppingCartCommand : IRequest<Response<ShoppingCartCommand>>
    {
        public int Id { get; set; }
        public int ApplicationUserId { get; set; }
    }
}
