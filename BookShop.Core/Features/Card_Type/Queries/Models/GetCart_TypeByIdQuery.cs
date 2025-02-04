using BookShop.Core.Bases;
using BookShop.Core.Features.Card_Type.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Card_Type.Queries.Models
{
    public class GetCart_TypeByIdQuery : IRequest<Response<GetSingleCart_TypeResponse>>
    {
        public GetCart_TypeByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
