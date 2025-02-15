using BookShop.Core.Features.Order_State.Queries.Response_DTO_;

namespace BookShop.Core.Mapping.Order_State
{
    public partial class Order_StateProfile
    {
        public void GetOrder_StateByIdMapping()
        {
            CreateMap<DataAccess.Entities.Order_State, GetSingleOrder_StateResponse>()
                .ForMember(dest => dest.Id, obtion => obtion.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, obtion => obtion.MapFrom(src => src.Localize(src.Name_Ar, src.Name)));
        }
    }
}
