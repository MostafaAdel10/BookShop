using BookShop.DataAccess.Entities.Identity;
using BookShop.Infrastructure.InfrastructureBases;

namespace BookShop.Infrastructure.Abstracts
{
    public interface IApplicationUserRepository : IGenericRepositoryAsync<ApplicationUser>
    {
        public Task<ApplicationUser> GetByUserNameAsync(string userName);
    }
}
