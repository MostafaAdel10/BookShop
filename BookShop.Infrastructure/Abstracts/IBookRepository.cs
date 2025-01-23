using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.InfrastructureBases;


namespace BookShop.Infrastructure.Abstracts
{
    public interface IBookRepository : IGenericRepositoryAsync<Book>
    {
        public Task<List<Book>> GetBooksListAsync();
        public Task<bool> SubSubjectRelatedWithBook(int Id);
        public Task<bool> SubjectRelatedWithBook(int Id);
    }
}
