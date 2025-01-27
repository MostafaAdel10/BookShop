using AutoMapper;

namespace BookShop.Core.Mapping.Discounts
{
    public partial class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            AddDiscountCommandMapping();
            DiscountCommandMapping();
            GetDiscountListMapping();
            GetDiscountByIdMapping();
            EditDiscountCommandMapping();
        }
    }
}
