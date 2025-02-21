using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface IApplicationUserService
    {
        public Task<ApplicationUser> GetByUserNameAsync(string userName);
        public Task<ApplicationUser> GetByIdAsync(int id);
        public Task<bool> IsUserIdIdExist(int userId);
    }
}
