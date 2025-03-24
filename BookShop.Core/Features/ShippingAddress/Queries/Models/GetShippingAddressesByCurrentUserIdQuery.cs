using BookShop.Core.Bases;
using BookShop.Core.Features.ShippingAddress.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.ShippingAddress.Queries.Models
{
    public class GetShippingAddressesByCurrentUserIdQuery : IRequest<Response<List<GetShippingAddressesByCurrentUserIdResponse>>>
    {
    }
}
