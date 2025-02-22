using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using BookShop.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repository
{
    public class OrderRepository : GenericRepositoryAsync<Order>, IOrderRepository
    {
        #region Fields
        private readonly DbSet<Order> _orders;
        #endregion

        #region Contractors
        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _orders = dbContext.Set<Order>();
        }
        #endregion

        #region Handle Functions
        public IQueryable<Order> GetByUserIdAsync(int userId)
        {
            return _orders.Where(order => order.ApplicationUserId == userId);
        }
        public async Task<List<Order>> GetOrdersListAsync()
        {
            return await _orders
                .Include(o => o.ApplicationUser)
                .Include(o => o.payment_Methods)
                .Include(o => o.shipping_Methods)
                .Include(o => o.order_State)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.book)
                .ToListAsync();
        }
        public async ValueTask<Order> GetOrderByIdAsyncWithInclude(int id)
        {
            var entity = _orders.AsQueryable();
            var order = await entity.Where(p => p.Id == id)
                .Include(p => p.OrderItems)
                !.ThenInclude(p => p.book)
                 .Include(p => p.ApplicationUser)
                 .Include(p => p.order_State)
                 .Include(p => p.shipping_Methods)
                 .Include(p => p.payment_Methods)
                 .FirstOrDefaultAsync();
            return order;
        }
        public async Task<int> MaxCode()
        {
            var maxCode = await _orders.MaxAsync(p => p.Code);
            return maxCode ?? 999;
        }
        #endregion
    }
}
