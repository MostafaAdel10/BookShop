using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;
using Microsoft.EntityFrameworkCore;


namespace BookShop.Service.Implementations
{
    public class BookService : IBookService
    {
        #region Fields
        private readonly IBookRepository _bookRepository;
        #endregion

        #region Contractors
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<List<Book>> GetBooksListAsync()
        {
            return await _bookRepository.GetBooksListAsync();
        }

        public async Task<Book> GetBookByIdWithIncludeAsync(int id)
        {
            //var book = _bookRepository.GetByIdAsync(id);
            var book = _bookRepository.GetTableNoTracking()
                .Include(s => s.Subject)
                .Include(sub => sub.SubSubject)
                .Where(b => b.Id == id)
                .FirstOrDefault();

            return book;
        }

        public async Task<string> AddAsync(Book book)
        {
            //Added Book
            await _bookRepository.AddAsync(book);
            return "Success";
        }

        public async Task<bool> IsISBNExist(string isbn)
        {
            //Check if the ISBN exists or not
            var book = _bookRepository.GetTableNoTracking().Where(b => b.ISBN13.Equals(isbn)).FirstOrDefault();
            if (book == null) return false;
            return true;
        }

        public async Task<bool> IsISBNExistExcludeSelf(string isbn, int id)
        {
            //Check if the ISBN exists or not
            var book = await _bookRepository.GetTableNoTracking().Where(b => b.ISBN13.Equals(isbn) & !b.Id.Equals(id)).FirstOrDefaultAsync();
            if (book == null) return false;
            return true;
        }

        public async Task<string> EditAsync(Book book)
        {
            await _bookRepository.UpdateAsync(book);
            return "Success";
        }

        public async Task<string> DeleteAsync(Book book)
        {
            var transaction = _bookRepository.BeginTransaction();

            try
            {
                await _bookRepository.DeleteAsync(book);
                await transaction.CommitAsync();
                return "Success";
            }
            catch
            {
                await transaction.RollbackAsync();
                return "Failed";
            }
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            return book;
        }
        #endregion



    }
}
