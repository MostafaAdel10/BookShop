using AutoMapper;

namespace BookShop.Core.Mapping.CartItem
{
    public partial class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            AddCartItemCommandMapping();
            CartItemCommandMapping();
            EditCartItemCommandMapping();
            GetCartItemListMapping();
            GetCartItemByIdMapping();
        }
    }
}
