using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface IOrder_StateService
    {
        public Task<Order_State> GetOrder_StateById(int id);
        public Task<List<Order_State>> GetOrder_StatesListAsync();
        public Task<string> AddAsync(Order_State order_State);
        public Task<string> EditAsync(Order_State order_State);
        public Task<string> DeleteAsync(Order_State order_State);
        public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
        public Task<bool> IsNameExistExcludeSelf(string name, int id);
        public Task<bool> IsNameArExist(string nameAr);
        public Task<bool> IsNameExist(string name);
    }
}
