using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.InfrastructureBases;

namespace BookShop.Infrastructure.Abstracts
{
    public interface ISubSubjectRepository : IGenericRepositoryAsync<SubSubject>
    {
        public Task<List<SubSubject>> GetSubSubjectsListAsync();
        public Task<bool> SubjectRelatedWithSubSubject(int Id);

    }
}
