using BookShop.Core.Bases;
using BookShop.Core.Features.Review.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Review.Queries.Models
{
    public class GetReviewByBookIdQuery : IRequest<Response<List<GetReviewListResponse>>>
    {
        public GetReviewByBookIdQuery(int bookId)
        {
            BookId = bookId;
        }
        public int BookId { get; set; }
    }
}
