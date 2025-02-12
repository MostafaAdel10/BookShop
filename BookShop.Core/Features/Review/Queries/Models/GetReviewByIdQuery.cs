using BookShop.Core.Bases;
using BookShop.Core.Features.Review.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Review.Queries.Models
{
    public class GetReviewByIdQuery : IRequest<Response<GetSingleReviewResponse>>
    {
        public GetReviewByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
