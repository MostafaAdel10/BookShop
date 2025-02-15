using BookShop.Core.Features.ShoppingCart.Commands.Models;

namespace BookShop.Core.Mapping.ShoppingCart
{
    public partial class ShoppingCartProfile
    {
        public void EditShoppingCartCommandMapping()
        {
            CreateMap<EditShoppingCartCommand, DataAccess.Entities.ShoppingCart>();
        }
    }
}
