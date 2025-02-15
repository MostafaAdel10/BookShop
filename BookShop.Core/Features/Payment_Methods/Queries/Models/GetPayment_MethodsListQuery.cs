using BookShop.Core.Bases;
using BookShop.Core.Features.Payment_Methods.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Payment_Methods.Queries.Models
{
    public class GetPayment_MethodsListQuery : IRequest<Response<List<GetPayment_MethodsListResponse>>>
    {
    }
}
