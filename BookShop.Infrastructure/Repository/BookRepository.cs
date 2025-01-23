using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using BookShop.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;


namespace BookShop.Infrastructure.Repository
{
    public class BookRepository : GenericRepositoryAsync<Book>, IBookRepository
    {
        #region Fields
        private readonly DbSet<Book> _books;
        #endregion

        #region Contractors
        public BookRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _books = dbContext.Set<Book>();
        }
        #endregion

        #region Handle Functions
        public async Task<List<Book>> GetBooksListAsync()
        {
            return await _books.Include(s => s.Subject).Include(sub => sub.SubSubject).ToListAsync();
        }

        public async Task<bool> SubjectRelatedWithBook(int Id)
        {
            return await _books.AnyAsync(s => s.SubjectId.Equals(Id));
        }

        public async Task<bool> SubSubjectRelatedWithBook(int Id)
        {
            return await _books.AnyAsync(ss => ss.SubSubjectId.Equals(Id));
        }
        #endregion


    }
}
