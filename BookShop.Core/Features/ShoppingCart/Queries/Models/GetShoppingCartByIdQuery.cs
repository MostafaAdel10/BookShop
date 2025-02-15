using BookShop.Core.Bases;
using BookShop.Core.Features.ShoppingCart.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.ShoppingCart.Queries.Models
{
    public class GetShoppingCartByIdQuery : IRequest<Response<GetSingleShoppingCartResponse>>
    {
        public GetShoppingCartByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
