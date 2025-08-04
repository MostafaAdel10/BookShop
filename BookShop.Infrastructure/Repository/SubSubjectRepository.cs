using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using BookShop.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repository
{
    public class SubSubjectRepository : GenericRepositoryAsync<SubSubject>, ISubSubjectRepository
    {
        #region Fields
        private readonly DbSet<SubSubject> _subSubject;
        #endregion

        #region Contractors
        public SubSubjectRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _subSubject = dbContext.Set<SubSubject>();
        }
        #endregion

        #region Handle Functions
        public async Task<List<SubSubject>> GetSubSubjectsListAsync()
        {
            return await _subSubject.ToListAsync();
        }

        public async Task<bool> SubjectRelatedWithSubSubject(int Id)
        {
            return await _subSubject.AnyAsync(s => s.SubjectId.Equals(Id));
        }
        #endregion
    }
}
