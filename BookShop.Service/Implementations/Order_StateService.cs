using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Service.Implementations
{
    public class Order_StateService : IOrder_StateService
    {
        #region Fields
        private readonly IOrder_StateRepository _order_StateRepository;
        #endregion

        #region Constructors
        public Order_StateService(IOrder_StateRepository order_StateRepository)
        {
            _order_StateRepository = order_StateRepository;
        }
        #endregion

        #region Handel Functions
        public async Task<string> AddAsync(Order_State order_State)
        {
            //Added order_State
            await _order_StateRepository.AddAsync(order_State);
            return "Success";
        }

        public async Task<string> DeleteAsync(Order_State order_State)
        {
            var transaction = _order_StateRepository.BeginTransaction();

            try
            {
                await _order_StateRepository.DeleteAsync(order_State);
                await transaction.CommitAsync();
                return "Success";
            }
            catch
            {
                await transaction.RollbackAsync();
                return "Failed";
            }
        }

        public async Task<string> EditAsync(Order_State order_State)
        {
            await _order_StateRepository.UpdateAsync(order_State);
            return "Success";
        }

        public async Task<Order_State> GetOrder_StateById(int id)
        {
            var order_State = await _order_StateRepository.GetByIdAsync(id);
            return order_State;
        }

        public async Task<List<Order_State>> GetOrder_StatesListAsync()
        {
            return await _order_StateRepository.GetOrder_StatesListAsync();
        }

        public async Task<bool> IsNameArExist(string nameAr)
        {
            //Check if the nameAr is Exist Or not
            var order_State = await _order_StateRepository.GetTableNoTracking().Where(x => x.Name_Ar.Equals(nameAr)).FirstOrDefaultAsync();
            if (order_State == null) return false;
            return true;
        }

        public async Task<bool> IsNameArExistExcludeSelf(string nameAr, int id)
        {
            //Check if the nameAr is Exist Or not
            var order_State = await _order_StateRepository.GetTableNoTracking().Where(x => x.Name_Ar.Equals(nameAr) & !x.Id.Equals(id)).FirstOrDefaultAsync();
            if (order_State == null) return false;
            return true;
        }

        public async Task<bool> IsNameExist(string name)
        {
            //Check if the nameAr is Exist Or not
            var order_State = await _order_StateRepository.GetTableNoTracking().Where(x => x.Name.Equals(name)).FirstOrDefaultAsync();
            if (order_State == null) return false;
            return true;
        }

        public async Task<bool> IsNameExistExcludeSelf(string name, int id)
        {
            //Check if the name is Exist Or not
            var order_State = await _order_StateRepository.GetTableNoTracking().Where(x => x.Name.Equals(name) & !x.Id.Equals(id)).FirstOrDefaultAsync();
            if (order_State == null) return false;
            return true;
        }

        public async Task<bool> IsOrderStateIdExist(int id)
        {
            //Check if the id is Exist Or not
            var order_State = await _order_StateRepository.GetTableNoTracking().Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            if (order_State == null) return false;
            return true;
        }
        #endregion
    }
}
