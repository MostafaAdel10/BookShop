using BookShop.Core.Bases;
using BookShop.Core.Features.Order_State.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Order_State.Queries.Models
{
    public class GetOrder_StateListQuery : IRequest<Response<List<GetOrder_StateListResponse>>>
    {
    }
}
