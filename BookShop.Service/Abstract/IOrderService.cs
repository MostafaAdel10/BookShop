using BookShop.DataAccess.Entities;
using BookShop.DataAccess.Helpers;

namespace BookShop.Service.Abstract
{
    public interface IOrderService : ICacheMemory<Order>
    {
        Task<bool> DeleteOrderAndOrderItemsAsync(int id);
        public Task<List<Order>> GetOrdersListAsync();
        public Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        public Task<Order> GetByIdAsync(int id);
        public Task<Order> GetByIdAsyncWithInclude(int id);
        public Task<string> AddAsync(Order order);
        public Task<Order> AddAsyncReturnId(Order order);
        public Task<bool> IsOrderIdExist(int id);
        public Task<bool> IsOrderIdExistWithUserId(int id, int userId);
        public Task<string> EditAsync(Order order);
        public Task<string> DeleteAsync(Order order);
        public IQueryable<Order> GetOrderQueryable();
        public IQueryable<Order> FilterOrderPaginatedQueryable(OrderOrderingEnum orderingEnum, string search);
        public Task<int> MaxCode();

    }
}
