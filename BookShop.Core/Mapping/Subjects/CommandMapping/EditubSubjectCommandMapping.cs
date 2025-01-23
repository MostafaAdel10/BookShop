using AutoMapper;
using BookShop.Core.Features.Subject.Commands.Models;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.Subjects
{
    public partial class SubjectProfile : Profile
    {
        public void EditSubjectCommandMapping()
        {
            CreateMap<EditSubjectCommand, Subject>()
                .ForMember(dest => dest.Id, obtion => obtion.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, obtion => obtion.MapFrom(src => src.Name))
                .ForMember(dest => dest.Name_Ar, obtion => obtion.MapFrom(src => src.Name_Ar));
        }
    }
}
