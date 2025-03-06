using BookShop.DataAccess.Entities;
using BookShop.DataAccess.Enums;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Service.Implementations
{
    public class ReviewService : IReviewService
    {
        #region Fields
        private readonly IReviewRepository _reviewRepository;
        #endregion

        #region Contractors
        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<string> AddAsync(Review review)
        {
            //Added Review
            await _reviewRepository.AddAsync(review);
            return "Success";
        }

        public async Task<string> DeleteAsync(Review review)
        {
            var transaction = _reviewRepository.BeginTransaction();

            try
            {
                await _reviewRepository.DeleteAsync(review);
                await transaction.CommitAsync();
                return "Success";
            }
            catch
            {
                await transaction.RollbackAsync();
                return "Failed";
            }
        }

        public async Task<string> EditAsync(Review review)
        {
            await _reviewRepository.UpdateAsync(review);
            return "Success";
        }

        public IQueryable<Review> FilterReviewPaginatedQueryable(ReviewOrderingEnum orderingEnum, string search)
        {
            var queryable = _reviewRepository.GetTableNoTracking()
                                      .AsQueryable();

            if (search != null)
            {
                queryable = queryable.Where(r => r.Content.Contains(search));
            }

            switch (orderingEnum)
            {
                case ReviewOrderingEnum.Id:
                    queryable = queryable.OrderBy(r => r.Id);
                    break;
                case ReviewOrderingEnum.Rating:
                    queryable = queryable.OrderBy(r => r.Rating);
                    break;
                case ReviewOrderingEnum.Content:
                    queryable = queryable.OrderBy(r => r.Content);
                    break;
                default:
                    queryable = queryable.OrderBy(b => b.Id);
                    break;
            }

            return queryable;
        }

        public async Task<Review> GetReviewByIdAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            return review;
        }

        public async Task<List<Review>> GetReviewsListAsync()
        {
            return await _reviewRepository.GetReviewsListAsync();
        }

        public async Task<IQueryable<Review>> GetReviewsListAsyncQueryble()
        {
            return await _reviewRepository.GetReviewsListAsyncQueryble();
        }

        public async Task<bool> IsBookRelatedWithReview(int bookId)
        {
            return await _reviewRepository.GetTableNoTracking().AnyAsync(d => d.BookId.Equals(bookId));
        }
        #endregion
    }
}
