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
        #endregion



    }
}
