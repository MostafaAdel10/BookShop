using BookShop.Core.Features.ShoppingCart.Commands.Models;

namespace BookShop.Core.Mapping.ShoppingCart
{
    public partial class ShoppingCartProfile
    {
        public void AddShoppingCartCommandMapping()
        {
            CreateMap<AddShoppingCartCommand, DataAccess.Entities.ShoppingCart>();
        }
    }
}
