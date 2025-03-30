using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Review.Queries.Models
{
    public class GetReviewByBookIdQuery : IRequest<Response<List<GetReviewsListByBookIdResponse>>>
    {
        public GetReviewByBookIdQuery(int bookId)
        {
            BookId = bookId;
        }
        public int BookId { get; set; }
    }
}
