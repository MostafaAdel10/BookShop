using BookShop.Core.Features.Books.Commands.Models;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.Books
{
    public partial class BookProfile
    {
        public void EditBookCommandMapping()
        {
            CreateMap<Book, EditBookCommand>();
        }
    }
}
