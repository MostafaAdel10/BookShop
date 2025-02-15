using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using BookShop.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repository
{
    public class Order_StateRepository : GenericRepositoryAsync<Order_State>, IOrder_StateRepository
    {
        #region Fields
        private readonly DbSet<Order_State> _order_State;
        #endregion

        #region Contractors
        public Order_StateRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _order_State = dbContext.Set<Order_State>();
        }
        #endregion

        #region Handle Functions
        public async Task<List<Order_State>> GetOrder_StatesListAsync()
        {
            return await _order_State.ToListAsync();
        }
        #endregion
    }
}
