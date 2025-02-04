using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface ICart_TypeService
    {
        public Task<List<Card_Type>> GetCart_TypesListAsync();
        public Task<Card_Type> GetCart_TypeByIdAsync(int id);
        public Task<string> AddAsync(Card_Type cart_Type);
        public Task<string> EditAsync(Card_Type cart_Type);
        public Task<string> DeleteAsync(Card_Type cart_Type);
        public Task<bool> IsNameExist(string name);
        public Task<bool> IsNameExistExcludeSelf(string name, int id);
    }
}
