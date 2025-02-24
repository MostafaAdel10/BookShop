using BookShop.Core.Features.Order.Queries.Response_DTO_;
using BookShop.Core.Wrappers;
using BookShop.DataAccess.Enums;
using MediatR;

namespace BookShop.Core.Features.Order.Queries.Models
{
    public class GetOrderPaginatedListQuery : IRequest<PaginatedResult<GetOrderPaginatedListResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public OrderOrderingEnum OrderBy { get; set; }
        public string? Search { get; set; }
    }
}
