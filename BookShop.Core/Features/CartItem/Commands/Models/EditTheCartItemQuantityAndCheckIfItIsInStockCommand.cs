using BookShop.Core.Bases;
using MediatR;

namespace BookShop.Core.Features.CartItem.Commands.Models
{
    public class EditTheCartItemQuantityAndCheckIfItIsInStockCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}