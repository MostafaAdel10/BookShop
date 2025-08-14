using BookShop.Core.Bases;
using BookShop.Core.Features.Order.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Order.Queries.Models
{
    public class GetOrdersByCurrentUserIdQuery : IRequest<Response<List<GetOrdersByCurrentUserIdResponse>>>
    {
    }
}
