using AutoMapper;

namespace BookShop.Core.Mapping.ShoppingCart
{
    public partial class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            AddShoppingCartCommandMapping();
            ShoppingCartCommandMapping();
            EditShoppingCartCommandMapping();
            GetShoppingCartListMapping();
            GetShoppingCartByIdMapping();
        }
    }
}
