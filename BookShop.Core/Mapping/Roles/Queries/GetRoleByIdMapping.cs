using BookShop.Core.Features.Authorization.Queries.Response_DTO_;
using BookShop.DataAccess.Entities.Identity;

namespace BookShop.Core.Mapping.Roles
{
    public partial class RoleProfile
    {
        public void GetRoleByIdMapping()
        {
            CreateMap<Role, GetRoleByIdResponse>();
        }
    }
}
