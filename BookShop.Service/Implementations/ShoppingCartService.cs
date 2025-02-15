using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;

namespace BookShop.Service.Implementations
{
    public class ShoppingCartService : IShoppingCartService
    {
        #region Fields
        private readonly IShoppingCartRepository _shoppingCartRepository;
        #endregion

        #region Contractors
        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<string> AddAsync(ShoppingCart shoppingCart)
        {
            //Added shoppingCart
            await _shoppingCartRepository.AddAsync(shoppingCart);
            return "Success";
        }

        public async Task<string> DeleteAsync(ShoppingCart shoppingCart)
        {
            var transaction = _shoppingCartRepository.BeginTransaction();

            try
            {
                await _shoppingCartRepository.DeleteAsync(shoppingCart);
                await transaction.CommitAsync();
                return "Success";
            }
            catch
            {
                await transaction.RollbackAsync();
                return "Failed";
            }
        }

        public async Task<string> EditAsync(ShoppingCart shoppingCart)
        {
            await _shoppingCartRepository.UpdateAsync(shoppingCart);
            return "Success";
        }

        public async Task<ShoppingCart> GetShoppingCartByIdAsync(int id)
        {
            var shoppingCart = await _shoppingCartRepository.GetByIdAsync(id);
            return shoppingCart;
        }

        public async Task<List<ShoppingCart>> GetShoppingCartsListAsync()
        {
            return await _shoppingCartRepository.GetShoppingCartsListAsync();
        }
        #endregion
    }
}
