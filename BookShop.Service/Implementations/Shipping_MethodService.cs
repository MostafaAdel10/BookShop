using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Service.Implementations
{
    public class Shipping_MethodService : IShipping_MethodService
    {
        #region Fields
        private readonly IShipping_MethodRepository _shipping_MethodRepository;
        #endregion

        #region Contractors
        public Shipping_MethodService(IShipping_MethodRepository shipping_MethodRepository)
        {
            _shipping_MethodRepository = shipping_MethodRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<string> AddAsync(Shipping_Methods shipping_Method)
        {
            //Added Shipping_Method
            await _shipping_MethodRepository.AddAsync(shipping_Method);
            return "Success";
        }

        public async Task<string> DeleteAsync(Shipping_Methods shipping_Method)
        {
            var transaction = _shipping_MethodRepository.BeginTransaction();

            try
            {
                await _shipping_MethodRepository.DeleteAsync(shipping_Method);
                await transaction.CommitAsync();
                return "Success";
            }
            catch
            {
                await transaction.RollbackAsync();
                return "Failed";
            }
        }

        public async Task<string> EditAsync(Shipping_Methods shipping_Method)
        {
            await _shipping_MethodRepository.UpdateAsync(shipping_Method);
            return "Success";
        }

        public async Task<Shipping_Methods> GetShipping_MethodByIdAsync(int id)
        {
            var shipping_Method = await _shipping_MethodRepository.GetByIdAsync(id);
            return shipping_Method;
        }

        public async Task<List<Shipping_Methods>> GetShipping_MethodsListAsync()
        {
            return await _shipping_MethodRepository.GetShipping_MethodsListAsync();
        }

        public async Task<bool> IsNameExist(string name)
        {
            //Check if the Method_Name is Exist Or not
            var shipping_Method = _shipping_MethodRepository.GetTableNoTracking().Where(x => x.Method_Name.Equals(name)).FirstOrDefault();
            if (shipping_Method == null) return false;
            return true;
        }

        public async Task<bool> IsNameExistExcludeSelf(string name, int id)
        {
            //Check if the Method_Name is Exist Or not
            var shipping_Method = await _shipping_MethodRepository.GetTableNoTracking().Where(x => x.Method_Name.Equals(name) & !x.Id.Equals(id)).FirstOrDefaultAsync();
            if (shipping_Method == null) return false;
            return true;
        }
        #endregion
    }
}
