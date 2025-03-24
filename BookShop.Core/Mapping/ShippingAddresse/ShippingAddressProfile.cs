using AutoMapper;

namespace BookShop.Core.Mapping.ShippingAddresse
{
    public partial class ShippingAddressProfile : Profile
    {
        public ShippingAddressProfile()
        {
            GetShippingAddressByCurrentUserIdMapping();
        }
    }
}
