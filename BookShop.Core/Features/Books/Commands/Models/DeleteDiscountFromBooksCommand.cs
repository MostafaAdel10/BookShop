using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.Books.Commands.Models
{
    public class DeleteDiscountFromBooksCommand : IRequest<Response<string>>
    {
        public int DiscountId { get; set; }
        public DeleteDiscountFromBooksCommand(int discountId)
        {
            DiscountId = discountId;
        }
    }
}
