using BookShop.Core.Features.ShippingAddress.Queries.Response_DTO_;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.ShippingAddresse
{
    public partial class ShippingAddressProfile
    {
        public void GetShippingAddressByCurrentUserIdMapping()
        {
            CreateMap<Address, GetShippingAddressesByCurrentUserIdResponse>();
        }
    }
}
