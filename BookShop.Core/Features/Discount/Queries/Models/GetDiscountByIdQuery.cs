using BookShop.Core.Bases;
using BookShop.Core.Features.Discount.Queries.Response_DTO_;
using MediatR;

namespace BookShop.Core.Features.Discount.Queries.Models
{
    public class GetDiscountByIdQuery : IRequest<Response<GetSingleDiscountResponse>>
    {
        public GetDiscountByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
