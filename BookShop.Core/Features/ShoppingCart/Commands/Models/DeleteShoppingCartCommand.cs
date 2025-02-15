using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.ShoppingCart.Commands.Models
{
    public class DeleteShoppingCartCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public DeleteShoppingCartCommand(int id)
        {
            Id = id;
        }
    }
}
