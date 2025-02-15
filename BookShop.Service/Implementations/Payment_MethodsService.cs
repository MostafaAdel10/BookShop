using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;

namespace BookShop.Service.Implementations
{
    public class Payment_MethodsService : IPayment_MethodsService
    {
        #region Fields
        private readonly IPayment_MethodsRepository _payment_MethodsRepository;
        #endregion

        #region Contractors
        public Payment_MethodsService(IPayment_MethodsRepository payment_MethodsRepository)
        {
            _payment_MethodsRepository = payment_MethodsRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<string> AddAsync(Payment_Methods payment_Method)
        {
            //Added Payment_Methods
            await _payment_MethodsRepository.AddAsync(payment_Method);
            return "Success";
        }

        public async Task<string> DeleteAsync(Payment_Methods payment_Method)
        {
            var transaction = _payment_MethodsRepository.BeginTransaction();

            try
            {
                await _payment_MethodsRepository.DeleteAsync(payment_Method);
                await transaction.CommitAsync();
                return "Success";
            }
            catch
            {
                await transaction.RollbackAsync();
                return "Failed";
            }
        }

        public async Task<string> EditAsync(Payment_Methods payment_Method)
        {
            await _payment_MethodsRepository.UpdateAsync(payment_Method);
            return "Success";
        }

        public async Task<Payment_Methods> GetPayment_MethodByIdAsync(int id)
        {
            var payment_Method = await _payment_MethodsRepository.GetByIdAsync(id);
            return payment_Method;
        }

        public async Task<List<Payment_Methods>> GetPayment_MethodsListAsync()
        {
            return await _payment_MethodsRepository.GetPayment_MethodsListAsync();
        }
        #endregion
    }
}
