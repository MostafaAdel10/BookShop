using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.InfrastructureBases;

namespace BookShop.Infrastructure.Abstracts
{
    public interface IShoppingCartRepository : IGenericRepositoryAsync<ShoppingCart>
    {
        public Task<List<ShoppingCart>> GetShoppingCartsListAsync();
    }
}
