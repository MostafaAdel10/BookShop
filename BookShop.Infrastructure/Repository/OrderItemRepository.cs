using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using BookShop.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repository
{
    public class OrderItemRepository : GenericRepositoryAsync<OrderItem>, IOrderItemRepository
    {
        #region Fields
        private readonly DbSet<OrderItem> _orderItem;
        #endregion

        #region Contractors
        public OrderItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _orderItem = dbContext.Set<OrderItem>();
        }
        #endregion

        #region Handle Functions
        public async Task<List<OrderItem>> GetOrderItemsListAsync()
        {
            return await _orderItem.ToListAsync();
        }
        #endregion
    }
}
