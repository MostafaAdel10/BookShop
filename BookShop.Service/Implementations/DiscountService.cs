using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Service.Implementations
{
    public class DiscountService : IDiscountService
    {
        #region Fields
        private readonly IDiscountRepository _discountRepository;
        #endregion

        #region Contractors
        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<string> AddAsync(Discount discount)
        {
            //Added Discount
            await _discountRepository.AddAsync(discount);
            return "Success";
        }

        public async Task<string> DeleteAsync(Discount discount)
        {
            var transaction = _discountRepository.BeginTransaction();

            try
            {
                await _discountRepository.DeleteAsync(discount);
                await transaction.CommitAsync();
                return "Success";
            }
            catch
            {
                await transaction.RollbackAsync();
                return "Failed";
            }
        }

        public async Task<string> EditAsync(Discount discount)
        {
            await _discountRepository.UpdateAsync(discount);
            return "Success";
        }

        public async Task<Discount> GetDiscountByIdAsync(int id)
        {
            var discount = await _discountRepository.GetByIdAsync(id);
            return discount;
        }

        public async Task<List<Discount>> GetDiscountsListAsync()
        {
            return await _discountRepository.GetDiscountsListAsync();
        }

        public async Task<bool> IsCodeExist(int? code)
        {
            //Check if the code is Exist Or not
            var discount = await _discountRepository.GetTableNoTracking().Where(x => x.Code.Equals(code)).FirstOrDefaultAsync();
            if (discount == null) return false;
            return true;
        }

        public async Task<bool> IsCodeExistExcludeSelf(int? code, int id)
        {
            //Check if the code is Exist Or not
            var discount = await _discountRepository.GetTableNoTracking().Where(x => x.Code.Equals(code) & !x.Id.Equals(id)).FirstOrDefaultAsync();
            if (discount == null) return false;
            return true;
        }

        public async Task<bool> IsDiscountExistById(int id)
        {
            //Check if the discount is Exist Or not
            var discount = await _discountRepository.GetTableNoTracking().Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            if (discount == null) return false;
            return true;
        }

        public async Task<bool> IsNameArExist(string nameAr)
        {
            //Check if the nameAr is Exist Or not
            var discount = await _discountRepository.GetTableNoTracking().Where(x => x.Name_Ar.Equals(nameAr)).FirstOrDefaultAsync();
            if (discount == null) return false;
            return true;
        }

        public async Task<bool> IsNameArExistExcludeSelf(string nameAr, int id)
        {
            //Check if the nameAr is Exist Or not
            var discount = await _discountRepository.GetTableNoTracking().Where(x => x.Name_Ar.Equals(nameAr) & !x.Id.Equals(id)).FirstOrDefaultAsync();
            if (discount == null) return false;
            return true;
        }

        public async Task<bool> IsNameExist(string name)
        {
            //Check if the nameAr is Exist Or not
            var discount = await _discountRepository.GetTableNoTracking().Where(x => x.Name.Equals(name)).FirstOrDefaultAsync();
            if (discount == null) return false;
            return true;
        }

        public async Task<bool> IsNameExistExcludeSelf(string name, int id)
        {
            //Check if the name is Exist Or not
            var discount = await _discountRepository.GetTableNoTracking().Where(x => x.Name.Equals(name) & !x.Id.Equals(id)).FirstOrDefaultAsync();
            if (discount == null) return false;
            return true;
        }
        #endregion

    }
}
