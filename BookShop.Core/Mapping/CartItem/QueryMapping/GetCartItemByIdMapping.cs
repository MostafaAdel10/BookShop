using BookShop.Core.Features.CartItem.Queries.Response_DTO_;

namespace BookShop.Core.Mapping.CartItem
{
    public partial class CartItemProfile
    {
        public void GetCartItemByIdMapping()
        {
            CreateMap<DataAccess.Entities.CartItem, GetSingleCartItemResponse>();
        }
    }
}
