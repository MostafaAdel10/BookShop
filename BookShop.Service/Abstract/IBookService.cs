using BookShop.DataAccess.Entities;
using BookShop.DataAccess.Helpers;


namespace BookShop.Service.Abstract
{
    public interface IBookService : ICacheMemory<Book>
    {
        public Task<List<Book>> GetBooksListAsync();
        public Task<Book> GetBookByIdWithIncludeAsync(int id);
        public Task<Book> GetByIdAsync(int id);
        public Task<string> AddAsync(Book book);
        public Task<Book> AddAsyncReturnId(Book book);
        public Task<bool> IsISBNExist(string isbn);
        public Task<bool> IsBookIdExist(int id);
        public Task<bool> IsISBNExistExcludeSelf(string isbn, int id);
        public Task<bool> IsSubjectIdExist(int subjectId);
        public Task<bool> IsSubSubjectIdExist(int subSubjectId);
        public Task<bool> IsQuantityGraterThanExist(int bookId, int quantity);
        public Task<bool> IsPriceTrueExist(int bookId, decimal price);
        public Task<bool> IsTheBookInStock(int bookId);
        public Task<string> EditAsync(Book book);
        public Task<string> DeleteAsync(Book book);
        public Task<bool> SubSubjectRelatedWithBook(int id);
        public Task<bool> SubjectRelatedWithBook(int id);
        public IQueryable<Book> GetBookQueryable();
        public IQueryable<Book> GetBookBySubjectIdQueryable(int SID);
        public IQueryable<Book> GetBookBySubSubjectIdQueryable(int SSID);
        public IQueryable<Book> FilterBookPaginatedQueryable(BookOrderingEnum orderingEnum, string search);

        public Task<string> EditUnit_InstockOfBookCommand(int bookId, int quantity, bool isSubtract = true);
    }
}
