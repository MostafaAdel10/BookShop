using BookShop.Core.Bases;
using BookShop.Core.Features.Shipping_Method.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Shipping_Method.Queries.Models
{
    public class GetShipping_MethodByIdQuery : IRequest<Response<GetSingleShipping_MethodResponse>>
    {
        public GetShipping_MethodByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
