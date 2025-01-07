using AutoMapper;


namespace BookShop.Core.Mapping.Books
{
    public partial class BookProfile : Profile
    {
        public BookProfile()
        {
            GetBookListMapping();
            GetBookByIdMapping();
        }
    }
}
