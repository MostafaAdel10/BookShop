using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;
using Microsoft.EntityFrameworkCore;

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
        public async Task<string> AddAsync(Book_Image book_Image)
        {
            //Added Card_Type
            await _book_ImageRepository.AddAsync(book_Image);
            return "Success";
        }

        public async Task<string> DeleteAsync(Book_Image book_Image)
        {
            var transaction = _book_ImageRepository.BeginTransaction();

            try
            {
                await _book_ImageRepository.DeleteAsync(book_Image);
                await transaction.CommitAsync();
                return "Success";
            }
            catch
            {
                await transaction.RollbackAsync();
                return "Failed";
            }
        }

        public async Task<string> EditAsync(Book_Image book_Image)
        {
            await _book_ImageRepository.UpdateAsync(book_Image);
            return "Success";
        }

        public async Task<List<Book_Image>> GetBook_ImagesByBookIdAsync(int bookId)
        {
            return await _book_ImageRepository.GetTableAsTracking().Where(x => x.BookId == bookId).ToListAsync();
        }

        public async Task<Book_Image> GetImageByBookIdAndImageUrlAsync(int bookId, string imageUrl)
        {
            return await _book_ImageRepository.GetTableAsTracking().Where(x => x.BookId == bookId && x.Image_url == imageUrl).FirstOrDefaultAsync();
        }
        #endregion
    }
}
