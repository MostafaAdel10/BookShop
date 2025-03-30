using BookShop.Core.Bases;
using BookShop.Core.Features.Review.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Review.Queries.Models
{
    public class GetReviewsByCurrentUserQuery : IRequest<Response<List<GetReviewListResponse>>>
    {
    }
}
