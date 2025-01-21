using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using BookShop.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repository
{
    public class SubjectRepository : GenericRepositoryAsync<Subject>, ISubjectRepository
    {
        #region Fields
        private readonly DbSet<Subject> _subject;
        #endregion

        #region Contractors
        public SubjectRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _subject = dbContext.Set<Subject>();
        }
        #endregion

        #region Handle Functions
        #endregion
    }
}
