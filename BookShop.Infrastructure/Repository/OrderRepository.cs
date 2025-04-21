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
        public IQueryable<Order> GetByUserId(int userId)
        {
            return _orders.Where(order => order.ApplicationUserId == userId);
        }
        public async Task<List<Order>> GetOrdersListAsync()
        {
            return await _orders
                .Include(order => order.Address)
                .Include(o => o.ApplicationUser)
                .Include(o => o.shipping_Methods)
                .Include(o => o.order_State)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.book)
                .ToListAsync();
        }
        public async ValueTask<Order> GetOrderByIdAsyncWithInclude(int id)
        {
            return await _orders
                .Where(p => p.Id == id)
                .Include(p => p.Address)
                .Include(p => p.ApplicationUser)
                .Include(p => p.order_State)
                .Include(p => p.shipping_Methods)
                .Include(p => p.OrderItems)
                    .ThenInclude(item => item.book)
                .FirstOrDefaultAsync();
        }
        #endregion
    }
}
