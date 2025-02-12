using BookShop.Core.Bases;
using BookShop.Core.Features.Review.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Review.Queries.Models
{
    public class GetReviewByUserIdQuery : IRequest<Response<List<GetReviewListResponse>>>
    {
        public GetReviewByUserIdQuery(int userId)
        {
            UserId = userId;
        }
        public int UserId { get; set; }
    }
}
