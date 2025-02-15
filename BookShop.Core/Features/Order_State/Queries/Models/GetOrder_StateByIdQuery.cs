using BookShop.Core.Bases;
using BookShop.Core.Features.Order_State.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Order_State.Queries.Models
{
    public class GetOrder_StateByIdQuery : IRequest<Response<GetSingleOrder_StateResponse>>
    {
        public GetOrder_StateByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
