using BookShop.Core.Bases;
using BookShop.Core.Features.Payment_Methods.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Payment_Methods.Queries.Models
{
    public class GetPayment_MethodsByIdQuery : IRequest<Response<GetSinglePayment_MethodsResponse>>
    {
        public GetPayment_MethodsByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
