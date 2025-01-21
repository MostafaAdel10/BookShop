using BookShop.Core.Features.Books.Queries.Response_DTO_;
using BookShop.Core.Wrappers;
using BookShop.DataAccess.Helpers;
using MediatR;

namespace BookShop.Core.Features.Books.Queries.Models
{
    public class GetBookPaginatedListQuery : IRequest<PaginatedResult<GetBookPaginatedListResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public BookOrderingEnum OrderBy { get; set; }
        public string? Search { get; set; }

    }
}
