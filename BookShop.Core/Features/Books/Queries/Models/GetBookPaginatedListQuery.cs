using BookShop.Core.Features.Books.Queries.Results;
using BookShop.Core.Wrappers;
using MediatR;

namespace BookShop.Core.Features.Books.Queries.Models
{
    public class GetBookPaginatedListQuery : IRequest<PaginatedResult<GetBookPaginatedListResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string[]? OrderBy { get; set; }
        public string? Search { get; set; }

    }
}
