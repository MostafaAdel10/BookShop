using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface IDiscountService
    {
        public Task<List<Discount>> GetDiscountsListAsync();
        public Task<Discount> GetDiscountByIdAsync(int id);
        public Task<string> AddAsync(Discount discount);
        public Task<string> EditAsync(Discount discount);
        public Task<string> DeleteAsync(Discount discount);
        public Task<bool> IsNameArExist(string nameAr);
        public Task<bool> IsNameExist(string name);
        public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
        public Task<bool> IsNameExistExcludeSelf(string name, int id);
    }
}
