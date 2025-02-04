using AutoMapper;

namespace BookShop.Core.Mapping.Cart_Type
{
    public partial class Cart_TypeProfile : Profile
    {
        public Cart_TypeProfile()
        {
            AddCart_TypeCommandMapping();
            Cart_TypeCommandMapping();
            EditCart_TypeCommandMapping();
            GetCart_TypeListMapping();
            GetCart_TypeByIdMapping();
        }
    }
}
