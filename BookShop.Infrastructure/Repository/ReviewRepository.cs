using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using BookShop.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repository
{
    internal class ReviewRepository : GenericRepositoryAsync<Review>, IReviewRepository
    {
        #region Fields
        private readonly DbSet<Review> _review;
        #endregion

        #region Contractors
        public ReviewRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _review = dbContext.Set<Review>();
        }
        #endregion

        #region Handle Functions
        public async Task<List<Review>> GetReviewsListAsync()
        {
            return await _review.ToListAsync();
        }

        public Task<IQueryable<Review>> GetReviewsListAsyncQueryble()
        {
            return Task.FromResult(
            _review
            .Include(r => r.UserReviews)
                .ThenInclude(ur => ur.applicationUser)
            .OrderByDescending(p => p.Created_at)
            .Where(entity => !entity.IsDeleted)
    );
        }

        public async Task<List<Review>> GetReviewsListWithIncludeAsync()
        {
            return await _review.Include(b => b.Book).Include(r => r.UserReviews).ToListAsync();
        }
        #endregion
    }
}
