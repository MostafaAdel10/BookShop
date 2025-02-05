using AutoMapper;

namespace BookShop.Core.Mapping.Shipping_Method
{
    public partial class Shipping_MethodProfile : Profile
    {
        public Shipping_MethodProfile()
        {
            AddShipping_MethodCommandMapping();
            Shipping_MethodCommandMapping();
            EditShipping_MethodCommandMapping();
            GetShipping_MethodListMapping();
            GetShipping_MethodByIdMapping();
        }
    }
}
