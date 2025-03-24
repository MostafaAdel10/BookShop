using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface IBook_ImageService
    {
        public Task<List<Book_Image>> GetBook_ImagesByBookIdAsync(int bookId);
        public Task<Book_Image> GetImageByBookIdAndImageUrlAsync(int bookId, string imageUrl);
        public Task<string> AddAsync(Book_Image book_Image);
        public Task<string> EditAsync(Book_Image book_Image);
        public Task<string> DeleteAsync(Book_Image book_Image);
    }
}
