using BookShop.Core.Features.CartItem.Commands.Models;

namespace BookShop.Core.Mapping.CartItem
{
    public partial class CartItemProfile
    {
        public void AddCartItemCommandMapping()
        {
            CreateMap<AddCartItemCommand, DataAccess.Entities.CartItem>();
        }
    }
}
