using BookShop.Core.Bases;
using BookShop.Core.Features.Order.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Order.Queries.Models
{
    public class GetOrderByIdQuery : IRequest<Response<GetOrderByIdResponse>>
    {
        public GetOrderByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
