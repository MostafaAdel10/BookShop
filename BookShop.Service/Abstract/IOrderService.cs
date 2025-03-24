using BookShop.DataAccess.Entities;
using BookShop.DataAccess.Enums;

namespace BookShop.Service.Abstract
{
    public interface IOrderService : ICacheMemory<Order>
    {
        public Task<Order?> GetOrderWithStateAndItemsAsync(int orderId);
        public Task<Order?> GetByIdWithIncludeAddressAsync(int orderId);
        public Task<bool> DeleteOrderAndOrderItemsAsync(int id);
        public Task<List<Order>> GetOrdersListAsync();
        public Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        public Task<Order> GetByIdAsync(int id);
        public Task<Order> GetByIdAsyncWithInclude(int id);
        public Task<string> AddAsync(Order order);
        public Task<Order> AddAsyncReturnId(Order order);
        public Task<bool> IsOrderIdExist(int id);
        public Task<string> EditAsync(Order order);
        public Task<string> DeleteAsync(Order order);
        public IQueryable<Order> GetOrderQueryable();
        public IQueryable<Order> FilterOrderPaginatedQueryable(OrderOrderingEnum orderingEnum, string search);
    }
}
