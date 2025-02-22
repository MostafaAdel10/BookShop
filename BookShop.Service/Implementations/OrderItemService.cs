using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;
using Microsoft.EntityFrameworkCore;

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
        public async Task<string> AddAsync(OrderItem orderItem)
        {
            //Added orderItem
            await _orderItemRepository.AddAsync(orderItem);
            return "Success";
        }

        public async Task<OrderItem> AddAsyncWithReturnId(OrderItem orderItem)
        {
            //Added orderItem
            return await _orderItemRepository.AddAsync(orderItem);
        }

        public async Task<string> DeleteAsync(OrderItem orderItem)
        {
            var transaction = _orderItemRepository.BeginTransaction();

            try
            {
                await _orderItemRepository.DeleteAsync(orderItem);
                await transaction.CommitAsync();
                return "Success";
            }
            catch
            {
                await transaction.RollbackAsync();
                return "Failed";
            }
        }

        public async Task<string> EditAsync(OrderItem orderItem)
        {
            await _orderItemRepository.UpdateAsync(orderItem);
            return "Success";
        }

        public async Task<OrderItem> GetOrderItemByIdAsync(int id)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);
            return orderItem;
        }

        public async Task<List<OrderItem>> GetOrderItemsListAsync()
        {
            return await _orderItemRepository.GetOrderItemsListAsync();
        }

        public async Task<bool> IsOrderItemIdExist(int id)
        {
            //Check if the OrderItem exists or not
            var order = await _orderItemRepository.GetTableNoTracking().Where(b => b.Id.Equals(id)).FirstOrDefaultAsync();
            if (order == null) return false;
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _orderItemRepository.SaveChangesAsync();
        }
        #endregion
    }
}
