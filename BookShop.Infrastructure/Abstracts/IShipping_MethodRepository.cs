using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.InfrastructureBases;

namespace BookShop.Infrastructure.Abstracts
{
    public interface IShipping_MethodRepository : IGenericRepositoryAsync<Shipping_Methods>
    {
        public Task<List<Shipping_Methods>> GetShipping_MethodsListAsync();
    }
}
