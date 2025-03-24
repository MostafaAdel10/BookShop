using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using BookShop.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repository
{
    public class ShoppingCartRepository : GenericRepositoryAsync<ShoppingCart>, IShoppingCartRepository
    {
        #region Fields
        private readonly DbSet<ShoppingCart> _shoppingCart;
        #endregion

        #region Contractors
        public ShoppingCartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _shoppingCart = dbContext.Set<ShoppingCart>();
        }
        #endregion

        #region Handle Functions
        public async Task<List<ShoppingCart>> GetShoppingCartsListAsync()
        {
            return await _shoppingCart.ToListAsync();
        }
        #endregion
    }
}