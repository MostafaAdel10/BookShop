using BookShop.Core.Features.CartItem.Commands.Models;

namespace BookShop.Core.Mapping.CartItem
{
    public partial class CartItemProfile
    {
        public void EditCartItemCommandMapping()
        {
            CreateMap<EditCartItemCommand, DataAccess.Entities.CartItem>();
        }
    }
}
