using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.InfrastructureBases;

namespace BookShop.Infrastructure.Abstracts
{
    public interface ISubjectRepository : IGenericRepositoryAsync<Subject>
    {
        public Task<List<Subject>> GetSubjectsListAsync();
    }
}
