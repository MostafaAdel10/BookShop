using BookShop.Core.Features.Subject.Queries.Response_DTO_;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.Subjects
{
    public partial class SubjectProfile
    {
        public void GetSubjectByIdMapping()
        {
            CreateMap<Subject, GetSubjectByIdResponse>()
                .ForMember(dest => dest.Name, obtion => obtion.MapFrom(src => src.Localize(src.Name_Ar, src.Name)))
                .ForMember(dest => dest.Id, obtion => obtion.MapFrom(src => src.Id))
                .ForMember(dest => dest.SubSubjectsList, obtion => obtion.MapFrom(src => src.SubSubjects));
            //.ForMember(dest => dest.BooksList, obtion => obtion.MapFrom(src => src.Books));

            CreateMap<SubSubject, GetSubSubjectListResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Localize(src.Name_Ar, src.Name)));

            //CreateMap<Book, GetBooksListResponse>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

        }
    }
}
