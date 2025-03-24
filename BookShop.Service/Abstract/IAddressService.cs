using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface IAddressService
    {
        public Task<List<Address>> GetAddressesByUserIdAsync(int userId);
    }
}
