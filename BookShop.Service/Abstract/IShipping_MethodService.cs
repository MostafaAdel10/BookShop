using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface IShipping_MethodService
    {
        public Task<List<Shipping_Methods>> GetShipping_MethodsListAsync();
        public Task<Shipping_Methods> GetShipping_MethodByIdAsync(int id);
        public Task<string> AddAsync(Shipping_Methods shipping_Method);
        public Task<string> EditAsync(Shipping_Methods shipping_Method);
        public Task<string> DeleteAsync(Shipping_Methods shipping_Method);
        public Task<bool> IsNameExist(string name);
        public Task<bool> IsNameExistExcludeSelf(string name, int id);
        public Task<bool> IsShippingMethodIdExist(int shippingMethodId);
    }
}
