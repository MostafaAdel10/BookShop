using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.InfrastructureBases;

namespace BookShop.Infrastructure.Abstracts
{
    public interface IOrderRepository : IGenericRepositoryAsync<Order>
    {
        public Task<List<Order>> GetOrdersListAsync();
        IQueryable<Order> GetByUserIdAsync(int userId);
        public ValueTask<Order> GetOrderByIdAsyncWithInclude(int id);
        public Task<int> MaxCode();
    }
}
