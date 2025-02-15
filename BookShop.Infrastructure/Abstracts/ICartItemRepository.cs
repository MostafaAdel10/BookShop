using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.InfrastructureBases;

namespace BookShop.Infrastructure.Abstracts
{
    public interface ICartItemRepository : IGenericRepositoryAsync<CartItem>
    {
        public Task<List<CartItem>> GetCartItemsListAsync();
    }
}
