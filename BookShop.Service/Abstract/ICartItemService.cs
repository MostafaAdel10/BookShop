using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface ICartItemService
    {
        public Task<bool> IsBookRelatedWithCartItem(int bookId);
        public Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId);
        public Task<List<CartItem>> GetCartItemsListAsync();
        public Task<CartItem> GetCartItemByIdAsync(int id);
        public Task<bool> IsCartItemExistByBookIdAndShoppingCartId(int bookId, int shoppingCartId);
        public Task<string> AddAsync(CartItem cartItem);
        public Task<string> EditAsync(CartItem cartItem);
        public Task<string> DeleteAsync(CartItem cartItem);
        public Task DeleteCartItemsByUserIdAsync(int userId);
    }
}