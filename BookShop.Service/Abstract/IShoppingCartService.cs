using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface IShoppingCartService
    {
        public Task<List<ShoppingCart>> GetShoppingCartsListAsync();
        public Task<ShoppingCart> GetShoppingCartByIdAsync(int id);
        public Task<ShoppingCart> GetByUserId(int userId);
        public Task<string> AddAsync(ShoppingCart shoppingCart);
        public Task<ShoppingCart> AddAsyncWithReturnId(ShoppingCart shoppingCart);
        public Task<string> EditAsync(ShoppingCart shoppingCart);
        public Task<string> DeleteAsync(ShoppingCart shoppingCart);
        public Task<bool> IsShoppingCartIdExist(int id);
        public Task<bool> UserHasCartAsync(int userId);
    }
}