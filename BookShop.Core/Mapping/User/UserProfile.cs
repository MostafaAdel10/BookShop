using AutoMapper;

namespace BookShop.Core.Mapping.User
{
    public partial class UserProfile : Profile
    {
        public UserProfile()
        {
            AddUserMapping();
            UserMapping();
        }
    }
}
