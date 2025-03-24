using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;

namespace BookShop.Service.Implementations
{
    public class OrderItemService : IOrderItemService
    {
        #region Fields
        private readonly IOrderItemRepository _orderItemRepository;
        #endregion

        #region Contractors
        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }
        #endregion

        #region Handle Functions
        public async Task AddRangeAsync(ICollection<OrderItem> orderItems)
        {
            await _orderItemRepository.AddRangeAsync(orderItems);
        }
        #endregion
    }
}
