using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using BookShop.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repository
{
    public class User_ReviewsRepository : GenericRepositoryAsync<User_Reviews>, IUser_ReviewsRepository
    {
        #region Fields
        private readonly DbSet<User_Reviews> _user_Reviews;
        #endregion

        #region Contractors
        public User_ReviewsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _user_Reviews = dbContext.Set<User_Reviews>();
        }
        #endregion

        #region Handle Functions
        #endregion
    }
}
