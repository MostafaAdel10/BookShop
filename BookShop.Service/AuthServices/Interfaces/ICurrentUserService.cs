using BookShop.DataAccess.Entities.Identity;

namespace BookShop.Service.AuthServices.Interfaces
{
    public interface ICurrentUserService
    {
        public Task<ApplicationUser> GetUserAsync();
        public int GetUserId();
        public Task<List<string>> GetCurrentUserRolesAsync();
    }
}
