using BookShop.DataAccess.Entities;


namespace BookShop.Service.Abstract
{
    public interface IBookService
    {
        public Task<List<Book>> GetBooksListAsync();
        public Task<Book> GetBookByIdAsync(int id);
    }
}
