using BookShop.DataAccess.Entities.Identity;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using BookShop.Service.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Service.Implementations
{
    public class ApplicationUserService : IApplicationUserService
    {
        #region Fields
        private readonly ApplicationDbContext _applicationDBContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailsService _emailsService;
        private readonly IUrlHelper _urlHelper;
        #endregion

        #region Contractors
        public ApplicationUserService(IApplicationUserRepository applicationUserRepository,
                                      UserManager<ApplicationUser> userManager,
                                      ApplicationDbContext applicationDBContext,
                                      IHttpContextAccessor httpContextAccessor,
                                      IUrlHelper urlHelper,
                                      IEmailsService emailsService)
        {
            _applicationUserRepository = applicationUserRepository;
            _userManager = userManager;
            _applicationDBContext = applicationDBContext;
            _httpContextAccessor = httpContextAccessor;
            _urlHelper = urlHelper;
            _emailsService = emailsService;
        }
        #endregion

        #region Handle Functions
        public async Task<string> AddUserAsync(ApplicationUser user, string password)
        {
            var trans = await _applicationDBContext.Database.BeginTransactionAsync();
            try
            {
                //if Email is Exist
                var existUser = await _userManager.FindByEmailAsync(user.Email);
                //email is Exist
                if (existUser != null) return "EmailIsExist";

                //if username is Exist
                var userByUserName = await _userManager.FindByNameAsync(user.UserName);
                //username is Exist
                if (userByUserName != null) return "UserNameIsExist";
                //Create
                var createResult = await _userManager.CreateAsync(user, password);
                //Failed
                if (!createResult.Succeeded)
                    return string.Join(",", createResult.Errors.Select(x => x.Description).ToList());

                await _userManager.AddToRoleAsync(user, "User");

                //Send Confirm Email
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var resquestAccessor = _httpContextAccessor.HttpContext.Request;
                var returnUrl = resquestAccessor.Scheme + "://" + resquestAccessor.Host + _urlHelper.Action("ConfirmEmail", "Authentication", new { userId = user.Id, code = code });
                var message = $"To Confirm Email Click Link: <a href='{returnUrl}'>Link Of Confirmation</a>";
                //$"/Api/V1/Authentication/ConfirmEmail?userId={user.Id}&code={code}";
                //message or body
                await _emailsService.SendEmail(user.Email, message, "ConFirm Email");

                await trans.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return "Failed";
            }
        }
        public async Task<ApplicationUser> GetByIdAsync(int id)
        {
            var applicationUser = await _applicationUserRepository.GetByIdAsync(id);
            return applicationUser;
        }
        public async Task<ApplicationUser> GetByUserNameAsync(string userName)
        {
            var applicationUser = await _applicationUserRepository.GetByUserNameAsync(userName);
            return applicationUser;
        }
        public async Task<bool> IsUserIdIdExist(int userId)
        {
            //Check if the applicationUserId is Exist Or not
            var applicationUser = await _applicationUserRepository.GetTableNoTracking().Where(s => s.Id.Equals(userId)).FirstOrDefaultAsync();
            if (applicationUser == null) return false;
            return true;
        }

        #endregion
    }
}
