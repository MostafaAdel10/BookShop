using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.InfrastructureBases;

namespace BookShop.Infrastructure.Abstracts
{
    public interface IDiscountRepository : IGenericRepositoryAsync<Discount>
    {
        public Task<List<Discount>> GetDiscountsListAsync();
    }
}
