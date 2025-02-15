using BookShop.Core.Features.ShoppingCart.Commands.Models;

namespace BookShop.Core.Mapping.ShoppingCart
{
    public partial class ShoppingCartProfile
    {
        public void ShoppingCartCommandMapping()
        {
            CreateMap<DataAccess.Entities.ShoppingCart, ShoppingCartCommand>();
        }
    }
}
