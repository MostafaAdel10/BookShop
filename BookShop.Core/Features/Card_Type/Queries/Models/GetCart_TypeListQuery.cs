using BookShop.Core.Bases;
using BookShop.Core.Features.Card_Type.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Card_Type.Queries.Models
{
    public class GetCart_TypeListQuery : IRequest<Response<List<GetCart_TypeListResponse>>>
    {
    }
}
