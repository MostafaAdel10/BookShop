using BookShop.Core.Features.Discount.Queries.Response_DTO_;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.Discounts
{
    public partial class DiscountProfile
    {
        public void GetDiscountByIdMapping()
        {
            CreateMap<Discount, GetSingleDiscountResponse>()
                .ForMember(dest => dest.Id, obtion => obtion.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, obtion => obtion.MapFrom(src => src.Localize(src.Name_Ar, src.Name)));
        }
    }
}
