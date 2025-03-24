using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.InfrastructureBases;

namespace BookShop.Infrastructure.Abstracts
{
    public interface IOrderRepository : IGenericRepositoryAsync<Order>
    {
        public Task<List<Order>> GetOrdersListAsync();
        public IQueryable<Order> GetByUserId(int userId);
        public ValueTask<Order> GetOrderByIdAsyncWithInclude(int id);
    }
}
