using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;

namespace BookShop.Service.Implementations
{
    public class User_ReviewsService : IUser_ReviewsService
    {
        #region Fields
        private readonly IUser_ReviewsRepository _user_ReviewsRepository;
        #endregion

        #region Contractors
        public User_ReviewsService(IUser_ReviewsRepository user_ReviewsRepository)
        {
            _user_ReviewsRepository = user_ReviewsRepository;
        }
        #endregion
        public async Task<string> AddAsync(User_Reviews user_Reviews)
        {
            //Added Card_Type
            await _user_ReviewsRepository.AddAsync(user_Reviews);
            return "Success";
        }
    }
}
