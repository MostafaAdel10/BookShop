using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface IOrderItemService
    {
        public Task AddRangeAsync(ICollection<OrderItem> orderItems);
    }
}
