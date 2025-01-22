using AutoMapper;
using BookShop.Core.Features.Subject.Commands.Models;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.Subjects
{
    public partial class SubjectProfile : Profile
    {
        public void AddSubjectCommandMapping()
        {
            CreateMap<AddSubjectCommand, Subject>()
                .ForMember(dest => dest.Name, obtion => obtion.MapFrom(src => src.Name))
                .ForMember(dest => dest.Name_Ar, obtion => obtion.MapFrom(src => src.Name_Ar));
        }
    }
}
