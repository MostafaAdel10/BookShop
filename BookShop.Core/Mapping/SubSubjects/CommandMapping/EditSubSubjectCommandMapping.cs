using AutoMapper;
using BookShop.Core.Features.SubSubject.Commands.Models;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.SubSubjects
{
    public partial class SubSubjectProfile : Profile
    {
        public void EditSubSubjectCommandMapping()
        {
            CreateMap<EditSubSubjectCommand, SubSubject>()
                .ForMember(dest => dest.Id, obtion => obtion.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, obtion => obtion.MapFrom(src => src.Name))
                .ForMember(dest => dest.Name_Ar, obtion => obtion.MapFrom(src => src.Name_Ar));
        }
    }
}
