using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface IPaymentService
    {
        public Task<string> AddAsync(Payment payment);
        public Task<string> EditAsync(Payment payment);
        public Task<Payment> GetByIdAsync(int id);
    }
}
