using BookShop.Core.Features.Order_State.Commands.Models;

namespace BookShop.Core.Mapping.Order_State
{
    public partial class Order_StateProfile
    {
        public void AddOrder_StateCommandMapping()
        {
            CreateMap<AddOrder_StateCommand, DataAccess.Entities.Order_State>()
                .ForMember(dest => dest.Name, obtion => obtion.MapFrom(src => src.Name))
                .ForMember(dest => dest.Name_Ar, obtion => obtion.MapFrom(src => src.Name_Ar));
        }
    }
}
