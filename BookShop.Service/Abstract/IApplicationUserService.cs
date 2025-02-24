using BookShop.DataAccess.Entities.Identity;

namespace BookShop.Service.Abstract
{
    public interface IApplicationUserService
    {
        public Task<string> AddUserAsync(ApplicationUser user, string password);
        public Task<ApplicationUser> GetByUserNameAsync(string userName);
        public Task<ApplicationUser> GetByIdAsync(int id);
        public Task<bool> IsUserIdIdExist(int userId);
    }
}
