using BookShop.DataAccess.Entities;


namespace BookShop.Service.Abstract
{
    public interface IBookService
    {
        public Task<List<Book>> GetBooksListAsync();
        public Task<Book> GetBookByIdWithIncludeAsync(int id);
        public Task<Book> GetByIdAsync(int id);
        public Task<string> AddAsync(Book book);
        public Task<bool> IsISBNExist(string isbn);
        public Task<bool> IsISBNExistExcludeSelf(string isbn, int id);
        public Task<string> EditAsync(Book book);
        public Task<string> DeleteAsync(Book book);
    }
}
