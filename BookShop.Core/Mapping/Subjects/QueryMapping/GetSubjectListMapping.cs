using AutoMapper;
using BookShop.Core.Features.Subject.Queries.Response_DTO_;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.Subjects
{
    public partial class SubjectProfile : Profile
    {
        public void GetSubjectListMapping()
        {
            CreateMap<Subject, GetSubjectListResponse>()
                .ForMember(dest => dest.Id, obtion => obtion.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, obtion => obtion.MapFrom(src => src.Localize(src.Name_Ar, src.Name)));
        }
    }
}
