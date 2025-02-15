using BookShop.Core.Features.ShoppingCart.Queries.Response_DTO_;

namespace BookShop.Core.Mapping.ShoppingCart
{
    public partial class ShoppingCartProfile
    {
        public void GetShoppingCartByIdMapping()
        {
            CreateMap<DataAccess.Entities.ShoppingCart, GetSingleShoppingCartResponse>();
        }
    }
}
