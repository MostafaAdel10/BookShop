using BookShop.Core.Bases;
using BookShop.Core.Features.CartItem.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.CartItem.Queries.Models
{
    public class GetCartItemListQuery : IRequest<Response<List<GetCartItemListResponse>>>
    {
    }
}
