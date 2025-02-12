using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.InfrastructureBases;

namespace BookShop.Infrastructure.Abstracts
{
    public interface IReviewRepository : IGenericRepositoryAsync<Review>
    {
        public Task<List<Review>> GetReviewsListAsync();
        public Task<IQueryable<Review>> GetReviewsListAsyncQueryble();
        public Task<List<Review>> GetReviewsListWithIncludeAsync();
    }
}
