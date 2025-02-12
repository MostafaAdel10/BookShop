using BookShop.Core.Features.Review.Queries.Response_DTO_;
using BookShop.Core.Wrappers;
using BookShop.DataAccess.Helpers;
using MediatR;

namespace BookShop.Core.Features.Review.Queries.Models
{
    public class GetReviewPaginatedListQuery : IRequest<PaginatedResult<GetReviewPaginatedListResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public ReviewOrderingEnum OrderBy { get; set; }
        public string? Search { get; set; }
    }
}
