using BookShop.Core.Features.CartItem.Commands.Models;

namespace BookShop.Core.Mapping.CartItem
{
    public partial class CartItemProfile
    {
        public void CartItemCommandMapping()
        {
            CreateMap<DataAccess.Entities.CartItem, CartItemCommand>();
        }
    }
}
