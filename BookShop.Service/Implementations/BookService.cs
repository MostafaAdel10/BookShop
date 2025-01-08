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

        public async Task<Book> GetBookByIdAsync(int id)
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
            //Check if the ISBN exists or not and if the ISBN is 13 number or not
            var bookResult = _bookRepository.GetTableNoTracking().Where(b => b.ISBN13.Equals(book.ISBN13)).FirstOrDefault();
            if (bookResult != null) return "ISBN must be unique.";
            if (book.ISBN13.Length > 13) return "ISBN must be 13 number.";
            if (book.ISBN13.Length < 13) return "ISBN must be 13 number.";
            //Added Book
            await _bookRepository.AddAsync(book);
            return "Success";
        }
        #endregion



    }
}
