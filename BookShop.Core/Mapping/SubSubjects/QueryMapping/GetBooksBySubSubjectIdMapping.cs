using BookShop.Core.Features.SubSubject.Queries.Response_DTO_;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.SubSubjects
{
    public partial class SubSubjectProfile
    {
        public void GetBooksBySubSubjectIdMapping()
        {
            CreateMap<SubSubject, GetBooksBySubSubjectIdResponse>()
                .ForMember(dest => dest.Name, obtion => obtion.MapFrom(src => src.Localize(src.Name_Ar, src.Name)))
                .ForMember(dest => dest.Id, obtion => obtion.MapFrom(src => src.Id))
                .ForMember(dest => dest.Subject, obtion => obtion.MapFrom(src => src.Subject));
            //.ForMember(dest => dest.BooksList, obtion => obtion.MapFrom(src => src.Books));

            CreateMap<Subject, GetSubjectResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Localize(src.Name_Ar, src.Name)));

            //CreateMap<Book, GetBooksListResponses>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
