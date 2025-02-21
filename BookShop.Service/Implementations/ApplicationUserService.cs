using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;

namespace BookShop.Service.Implementations
{
    public class ApplicationUserService : IApplicationUserService
    {
        #region Fields
        private readonly IApplicationUserRepository _applicationUserRepository;
        #endregion

        #region Contractors
        public ApplicationUserService(IApplicationUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;
        }

        public async Task<ApplicationUser> GetByIdAsync(int id)
        {
            var applicationUser = await _applicationUserRepository.GetByIdAsync(id);
            return applicationUser;
        }
        #endregion

        #region Handle Functions
        public async Task<ApplicationUser> GetByUserNameAsync(string userName)
        {
            var applicationUser = await _applicationUserRepository.GetByUserNameAsync(userName);
            return applicationUser;
        }

        public async Task<bool> IsUserIdIdExist(int userId)
        {
            //Check if the applicationUserId is Exist Or not
            var applicationUser = _applicationUserRepository.GetTableNoTracking().Where(s => s.Id.Equals(userId)).FirstOrDefault();
            if (applicationUser == null) return false;
            return true;
        }
        #endregion
    }
}
