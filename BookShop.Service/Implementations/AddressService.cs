using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Service.Implementations
{
    public class AddressService : IAddressService
    {
        #region Fields
        private readonly IAddressRepository _addressRepository;
        private readonly IOrderRepository _orderRepository;
        #endregion

        #region Contractors
        public AddressService(IAddressRepository addressRepository, IOrderRepository orderRepository)
        {
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<List<Address>> GetAddressesByUserIdAsync(int userId)
        {
            return await _orderRepository.GetTableNoTracking()
                                 .Where(o => o.ApplicationUserId == userId)
                                 .Select(o => o.Address) // جلب العناوين فقط
                                 .Distinct() // إزالة التكرارات في حال كان المستخدم لديه أكثر من طلب بنفس العنوان
                                 .ToListAsync();
        }
        #endregion
    }
}
