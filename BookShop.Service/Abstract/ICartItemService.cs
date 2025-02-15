using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface ICartItemService
    {
        public Task<List<CartItem>> GetCartItemsListAsync();
        public Task<CartItem> GetCartItemByIdAsync(int id);
        public Task<string> AddAsync(CartItem cartItem);
        public Task<string> EditAsync(CartItem cartItem);
        public Task<string> DeleteAsync(CartItem cartItem);
    }
}
