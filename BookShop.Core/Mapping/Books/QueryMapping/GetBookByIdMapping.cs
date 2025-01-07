using BookShop.Core.Features.Books.Queries.Results;
using BookShop.DataAccess.Entities;


namespace BookShop.Core.Mapping.Books
{
    public partial class BookProfile
    {
        public void GetBookByIdMapping()
        {
            CreateMap<Book, GetSingleBookResponse>()
                .ForMember(dest => dest.SubjectName, obtion => obtion.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.SubSubjectName, obtion => obtion.MapFrom(src => src.SubSubject.Name));
        }
    }
}
