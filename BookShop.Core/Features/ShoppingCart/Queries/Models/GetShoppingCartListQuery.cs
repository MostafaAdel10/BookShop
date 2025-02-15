using BookShop.Core.Bases;
using BookShop.Core.Features.ShoppingCart.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.ShoppingCart.Queries.Models
{
    public class GetShoppingCartListQuery : IRequest<Response<List<GetShoppingCartListResponse>>>
    {
    }
}
