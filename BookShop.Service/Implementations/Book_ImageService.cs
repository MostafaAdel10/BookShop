using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;

namespace BookShop.Service.Implementations
{
    public class Book_ImageService : IBook_ImageService
    {
        #region Fields
        private readonly IBook_ImageRepository _book_ImageRepository;
        #endregion

        #region Contractors
        public Book_ImageService(IBook_ImageRepository book_ImageRepository)
        {
            _book_ImageRepository = book_ImageRepository;
        }
        #endregion

        #region Handle Functions
        public async Task SaveChangesAsync()
        {
            await _book_ImageRepository.SaveChangesAsync();
        }
        #endregion
    }
}
