using BookShop.Core.Bases;
using BookShop.Core.Features.Order.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Order.Queries.Models
{
    public class GetOrdersByUserIdQuery : IRequest<Response<List<GetOrdersByUserIdResponse>>>
    {
        public GetOrdersByUserIdQuery(int userId)
        {
            UserId = userId;
        }
        public int UserId { get; set; }
    }
}
