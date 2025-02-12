using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.InfrastructureBases;

namespace BookShop.Infrastructure.Abstracts
{
    public interface IApplicationUserRepository : IGenericRepositoryAsync<ApplicationUser>
    {
        public Task<ApplicationUser> GetByUserNameAsync(string userName);
    }
}
