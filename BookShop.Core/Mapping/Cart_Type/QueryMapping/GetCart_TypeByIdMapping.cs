using BookShop.Core.Features.Card_Type.Queries.Response_DTO_;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.Cart_Type
{
    public partial class Cart_TypeProfile
    {
        public void GetCart_TypeByIdMapping()
        {
            CreateMap<Card_Type, GetSingleCart_TypeResponse>();
        }
    }
}
