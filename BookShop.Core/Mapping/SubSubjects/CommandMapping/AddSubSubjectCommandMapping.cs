using AutoMapper;
using BookShop.Core.Features.SubSubject.Commands.Models;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.SubSubjects
{
    public partial class SubSubjectProfile : Profile
    {
        public void AddSubSubjectCommandMapping()
        {
            CreateMap<AddSubSubjectCommand, SubSubject>()
                .ForMember(dest => dest.Name, obtion => obtion.MapFrom(src => src.Name))
                .ForMember(dest => dest.Name_Ar, obtion => obtion.MapFrom(src => src.Name_Ar))
                .ForMember(dest => dest.SubjectId, obtion => obtion.MapFrom(src => src.SubjectId));
        }
    }
}
