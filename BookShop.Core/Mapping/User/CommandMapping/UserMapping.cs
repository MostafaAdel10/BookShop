using BookShop.Core.Features.User.Commands.Models;
using BookShop.DataAccess.Entities.Identity;

namespace BookShop.Core.Mapping.User
{
    public partial class UserProfile
    {
        public void UserMapping()
        {
            CreateMap<ApplicationUser, UserCommand>();
        }
    }
}
