using BookShop.Core.Features.User.Queries.Response_DTO_;
using BookShop.DataAccess.Entities.Identity;

namespace BookShop.Core.Mapping.User
{
    public partial class UserProfile
    {
        public void GetUserPaginationMapping()
        {
            CreateMap<ApplicationUser, GetUserPaginationReponse>();

        }
    }
}
