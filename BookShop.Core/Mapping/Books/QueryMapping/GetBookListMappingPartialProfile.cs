using BookShop.Core.Features.Books.Queries.Response_DTO_;
using BookShop.DataAccess.Entities;


namespace BookShop.Core.Mapping.Books
{
    public partial class BookProfile
    {
        public void GetBookListMapping()
        {
            CreateMap<Book, GetBookListResponse>()
                .ForMember(dest => dest.SubjectName, obtion => obtion.MapFrom(src => src.Subject.Localize(src.Subject.Name_Ar, src.Subject.Name)))
                .ForMember(dest => dest.SubSubjectName, obtion => obtion.MapFrom(src => src.SubSubject.Localize(src.SubSubject.Name_Ar, src.SubSubject.Name)));
        }
    }
}
