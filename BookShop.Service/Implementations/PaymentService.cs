using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;

namespace BookShop.Service.Implementations
{
    public class PaymentService : IPaymentService
    {
        #region Fields
        private readonly IPaymentRepository _paymentRepository;
        #endregion

        #region Contractors
        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<string> AddAsync(Payment payment)
        {
            //Added Shipping_Method
            await _paymentRepository.AddAsync(payment);
            return "Success";
        }

        public async Task<string> EditAsync(Payment payment)
        {
            await _paymentRepository.UpdateAsync(payment);
            return "Success";
        }

        public async Task<Payment> GetByIdAsync(int id)
        {
            return await _paymentRepository.GetByIdAsync(id);
        }
        #endregion
    }
}
