using BookShop.DataAccess.Entities;
using BookShop.DataAccess.Enums;

namespace BookShop.Service.Abstract
{
    public interface IReviewService
    {
        public Task<List<Review>> GetReviewsListAsync();
        public Task<IQueryable<Review>> GetReviewsListAsyncQueryble();
        public Task<Review> GetReviewByIdAsync(int id);
        public Task<Review> GetReviewByIdAsyncWithInclude(int id);
        public Task<string> AddAsync(Review review);
        public Task<Review> AddAsyncWithReturnId(Review review);
        public Task<string> EditAsync(Review review);
        public Task<string> DeleteAsync(Review review);
        public IQueryable<Review> FilterReviewPaginatedQueryable(ReviewOrderingEnum orderingEnum, string search);
        public Task<bool> IsBookRelatedWithReview(int bookId);

    }
}
