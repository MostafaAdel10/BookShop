using BookShop.Core.Features.Order_State.Commands.Models;

namespace BookShop.Core.Mapping.Order_State
{
    public partial class Order_StateProfile
    {
        public void EditOrder_StateCommandMapping()
        {
            CreateMap<EditOrder_StateCommand, DataAccess.Entities.Order_State>()
                .ForMember(dest => dest.Id, obtion => obtion.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, obtion => obtion.MapFrom(src => src.Name))
                .ForMember(dest => dest.Name_Ar, obtion => obtion.MapFrom(src => src.Name_Ar));
        }
    }
}
