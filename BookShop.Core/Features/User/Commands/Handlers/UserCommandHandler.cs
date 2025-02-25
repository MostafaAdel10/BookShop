using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.User.Commands.Models;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities.Identity;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.User.Commands.Handlers
{
    public class UserCommandHandler : ResponseHandler,
        IRequestHandler<AddUserCommand, Response<string>>,
        IRequestHandler<EditUserCommand, Response<string>>
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _sharedResources;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUserService _applicationUserService;
        #endregion

        #region Constructors
        public UserCommandHandler(IMapper mapper, UserManager<ApplicationUser> userManager,
            IStringLocalizer<SharedResources> sharedResources, IApplicationUserService applicationUserService) : base(sharedResources)
        {
            _mapper = mapper;
            _sharedResources = sharedResources;
            _userManager = userManager;
            _applicationUserService = applicationUserService;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var identityUser = _mapper.Map<ApplicationUser>(request);
            //Create
            var createResult = await _applicationUserService.AddUserAsync(identityUser, request.Password);

            switch (createResult)
            {
                case "EmailIsExist": return BadRequest<string>(_sharedResources[SharedResourcesKeys.EmailIsExist]);
                case "UserNameIsExist": return BadRequest<string>(_sharedResources[SharedResourcesKeys.UserNameIsExist]);
                case "ErrorInCreateUser": return BadRequest<string>(_sharedResources[SharedResourcesKeys.FaildToAddUser]);
                case "Failed": return BadRequest<string>(_sharedResources[SharedResourcesKeys.TryToRegisterAgain]);
                case "Success": return Success("");
                default: return BadRequest<string>(createResult);
            }
        }

        public async Task<Response<string>> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            //check if user is exist
            var oldUser = await _userManager.FindByIdAsync(request.Id.ToString());
            //if Not Exist notfound
            if (oldUser == null) return NotFound<string>();
            //mapping
            var newUser = _mapper.Map(request, oldUser);

            //if username is Exist
            var userByUserName = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == newUser.UserName && x.Id != newUser.Id);
            //username is Exist
            if (userByUserName != null) return BadRequest<string>(_sharedResources[SharedResourcesKeys.UserNameIsExist]);

            //update
            var result = await _userManager.UpdateAsync(newUser);
            //result is not success
            if (!result.Succeeded) return BadRequest<string>(_sharedResources[SharedResourcesKeys.UpdateFailed]);
            //message
            return Success((string)_sharedResources[SharedResourcesKeys.Updated]);
        }
        #endregion

    }
}
