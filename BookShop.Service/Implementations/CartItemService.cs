using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Service.Implementations
{
    public class CartItemService : ICartItemService
    {
        #region Fields
        private readonly ICartItemRepository _cartItemRepository;
        #endregion

        #region Contractors
        public CartItemService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<bool> IsBookRelatedWithCartItem(int bookId)
        {
            //Check if the book related with cart item exists or not
            var book = await _cartItemRepository.GetTableNoTracking().Where(b => b.BookId.Equals(bookId)).FirstOrDefaultAsync();
            if (book == null) return false;
            return true;
        }
        public async Task<string> AddAsync(CartItem cartItem)
        {
            //Added cartItem
            await _cartItemRepository.AddAsync(cartItem);
            return "Success";
        }

        public async Task<string> DeleteAsync(CartItem cartItem)
        {
            var transaction = _cartItemRepository.BeginTransaction();

            try
            {
                await _cartItemRepository.DeleteAsync(cartItem);
                await transaction.CommitAsync();
                return "Success";
            }
            catch
            {
                await transaction.RollbackAsync();
                return "Failed";
            }
        }

        public async Task DeleteCartItemsByUserIdAsync(int userId)
        {
            await _cartItemRepository.GetTableAsTracking()
                .Where(ci => ci.ShoppingCart.ApplicationUserId == userId)
                .ExecuteDeleteAsync();
        }

        public async Task<string> EditAsync(CartItem cartItem)
        {
            await _cartItemRepository.UpdateAsync(cartItem);
            return "Success";
        }

        public async Task<CartItem> GetCartItemByIdAsync(int id)
        {
            var cartItem = await _cartItemRepository.GetByIdAsync(id);
            return cartItem;
        }

        public async Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId)
        {
            return await _cartItemRepository.GetCartItemsByUserIdAsync(userId);
        }

        public async Task<List<CartItem>> GetCartItemsListAsync()
        {
            return await _cartItemRepository.GetCartItemsListAsync();
        }

        public async Task<bool> IsCartItemExistByBookIdAndShoppingCartId(int bookId, int shoppingCartId)
        {
            return await _cartItemRepository.GetTableNoTracking().AnyAsync(x => x.BookId == bookId && x.ShoppingCartId == shoppingCartId);
        }
        #endregion
    }
}