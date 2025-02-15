using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface IPayment_MethodsService
    {
        public Task<List<Payment_Methods>> GetPayment_MethodsListAsync();
        public Task<Payment_Methods> GetPayment_MethodByIdAsync(int id);
        public Task<string> AddAsync(Payment_Methods payment_Method);
        public Task<string> EditAsync(Payment_Methods payment_Method);
        public Task<string> DeleteAsync(Payment_Methods payment_Method);
    }
}
