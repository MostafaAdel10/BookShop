using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using BookShop.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repository
{
    public class CartItemRepository : GenericRepositoryAsync<CartItem>, ICartItemRepository
    {
        #region Fields
        private readonly DbSet<CartItem> _cartItem;
        #endregion

        #region Contractors
        public CartItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _cartItem = dbContext.Set<CartItem>();
        }
        #endregion

        #region Handle Functions
        public async Task<List<CartItem>> GetCartItemsListAsync()
        {
            return await _cartItem.ToListAsync();
        }
        #endregion
    }
}
