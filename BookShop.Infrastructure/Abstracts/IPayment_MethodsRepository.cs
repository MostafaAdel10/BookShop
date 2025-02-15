using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.InfrastructureBases;

namespace BookShop.Infrastructure.Abstracts
{
    public interface IPayment_MethodsRepository : IGenericRepositoryAsync<Payment_Methods>
    {
        public Task<List<Payment_Methods>> GetPayment_MethodsListAsync();
    }
}
