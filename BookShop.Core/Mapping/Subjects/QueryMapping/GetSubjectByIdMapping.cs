using BookShop.Core.Features.Subject.Queries.Response_DTO_;

namespace BookShop.Core.Mapping.Subjects
{
    public partial class SubjectProfile
    {
        public void GetSubjectByIdMapping()
        {
            CreateMap<DataAccess.Entities.Subject, GetSubjectByIdResponse>()
                .ForMember(dest => dest.Id, obtion => obtion.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, obtion => obtion.MapFrom(src => src.Localize(src.Name_Ar, src.Name)));
        }
    }
}
