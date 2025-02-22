using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface IOrderItemService
    {
        public Task<List<OrderItem>> GetOrderItemsListAsync();
        public Task<OrderItem> GetOrderItemByIdAsync(int id);
        public Task<bool> IsOrderItemIdExist(int id);
        public Task<string> AddAsync(OrderItem orderItem);
        public Task<OrderItem> AddAsyncWithReturnId(OrderItem orderItem);
        public Task<string> EditAsync(OrderItem orderItem);
        public Task<string> DeleteAsync(OrderItem orderItem);
        public Task SaveChangesAsync();
    }
}
