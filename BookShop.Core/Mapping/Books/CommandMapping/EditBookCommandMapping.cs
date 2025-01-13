using BookShop.Core.Features.Books.Commands.Models;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.Books
{
    public partial class BookProfile
    {
        public void EditBookCommandMapping()
        {
            CreateMap<EditBookCommand, Book>()
                .ForMember(dest => dest.SubjectId, obtion => obtion.MapFrom(src => src.SubjectId))
                .ForMember(dest => dest.SubSubjectId, obtion => obtion.MapFrom(src => src.SubSubjectId))
                .ForMember(dest => dest.Id, obtion => obtion.MapFrom(src => src.Id));
        }
    }
}
