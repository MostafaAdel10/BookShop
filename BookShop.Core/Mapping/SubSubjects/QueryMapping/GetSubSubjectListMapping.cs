using AutoMapper;
using BookShop.Core.Features.SubSubject.Queries.Response_DTO_;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.SubSubjects
{
    public partial class SubSubjectProfile : Profile
    {
        public void GetSubSubjectListMapping()
        {
            CreateMap<SubSubject, GetSubSubjectListResponses>()
                .ForMember(dest => dest.Id, obtion => obtion.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, obtion => obtion.MapFrom(src => src.Localize(src.Name_Ar, src.Name)));
        }
    }
}
