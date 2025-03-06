using BookShop.Core.Features.SubSubject.Queries.Response_DTO_;

namespace BookShop.Core.Mapping.SubSubjects
{
    public partial class SubSubjectProfile
    {
        public void GetSubSubjectByIdMapping()
        {
            CreateMap<DataAccess.Entities.SubSubject, GetSubSubjectByIdResponse>()
                .ForMember(dest => dest.Id, obtion => obtion.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, obtion => obtion.MapFrom(src => src.Localize(src.Name_Ar, src.Name)));
        }
    }
}
