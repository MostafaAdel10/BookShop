using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using BookShop.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repository
{
    public class ApplicationUserRepository : GenericRepositoryAsync<ApplicationUser>, IApplicationUserRepository
    {
        #region Fields
        private readonly DbSet<ApplicationUser> _applicationUser;
        #endregion

        #region Contractors
        public ApplicationUserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _applicationUser = dbContext.Set<ApplicationUser>();
        }
        #endregion

        #region Handle Functions
        public async Task<ApplicationUser> GetByUserNameAsync(string userName)
        {
            return await _applicationUser.FirstOrDefaultAsync(p => p.UserName == userName);
        }
        #endregion
    }
}
