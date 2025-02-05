using BookShop.Core.Bases;
using BookShop.Core.Features.Shipping_Method.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Shipping_Method.Queries.Models
{
    public class GetShipping_MethodListQuery : IRequest<Response<List<GetShipping_MethodListResponse>>>
    {
    }
}
