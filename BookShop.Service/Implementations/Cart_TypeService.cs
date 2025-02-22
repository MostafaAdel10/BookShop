using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Service.Implementations
{
    public class Cart_TypeService : ICart_TypeService
    {
        #region Fields
        private readonly ICart_TypeRepository _cart_TypeRepository;
        #endregion

        #region Contractors
        public Cart_TypeService(ICart_TypeRepository cart_TypeRepository)
        {
            _cart_TypeRepository = cart_TypeRepository;
        }
        #endregion
        public async Task<string> AddAsync(Card_Type cart_Type)
        {
            //Added Card_Type
            await _cart_TypeRepository.AddAsync(cart_Type);
            return "Success";
        }

        public async Task<string> DeleteAsync(Card_Type cart_Type)
        {
            var transaction = _cart_TypeRepository.BeginTransaction();

            try
            {
                await _cart_TypeRepository.DeleteAsync(cart_Type);
                await transaction.CommitAsync();
                return "Success";
            }
            catch
            {
                await transaction.RollbackAsync();
                return "Failed";
            }
        }

        public async Task<string> EditAsync(Card_Type cart_Type)
        {
            await _cart_TypeRepository.UpdateAsync(cart_Type);
            return "Success";
        }

        public async Task<Card_Type> GetCart_TypeByIdAsync(int id)
        {
            var card_Type = await _cart_TypeRepository.GetByIdAsync(id);
            return card_Type;
        }

        public async Task<List<Card_Type>> GetCart_TypesListAsync()
        {
            return await _cart_TypeRepository.GetCard_TypesListAsync();
        }

        public async Task<bool> IsNameExist(string name)
        {
            //Check if the nameAr is Exist Or not
            var card_Type = await _cart_TypeRepository.GetTableNoTracking().Where(x => x.Name.Equals(name)).FirstOrDefaultAsync();
            if (card_Type == null) return false;
            return true;
        }

        public async Task<bool> IsNameExistExcludeSelf(string name, int id)
        {
            //Check if the name is Exist Or not
            var card_Type = await _cart_TypeRepository.GetTableNoTracking().Where(x => x.Name.Equals(name) & !x.Id.Equals(id)).FirstOrDefaultAsync();
            if (card_Type == null) return false;
            return true;
        }
    }
}
