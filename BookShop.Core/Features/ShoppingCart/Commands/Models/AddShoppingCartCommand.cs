using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.ShoppingCart.Commands.Models
{
    public class AddShoppingCartCommand : IRequest<Response<ShoppingCartCommand>>
    {
        public int ApplicationUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
