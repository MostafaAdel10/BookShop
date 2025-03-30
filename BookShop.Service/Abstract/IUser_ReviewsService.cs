using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface IUser_ReviewsService
    {
        public Task<string> AddAsync(User_Reviews user_Reviews);
    }
}
