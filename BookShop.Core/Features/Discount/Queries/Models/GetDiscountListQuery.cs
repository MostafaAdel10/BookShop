using BookShop.Core.Bases;
using BookShop.Core.Features.Discount.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Discount.Queries.Models
{
    public class GetDiscountListQuery : IRequest<Response<List<GetDiscountListResponse>>>
    {
    }
}
