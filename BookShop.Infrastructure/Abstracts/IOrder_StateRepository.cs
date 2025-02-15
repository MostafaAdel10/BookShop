using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.InfrastructureBases;

namespace BookShop.Infrastructure.Abstracts
{
    public interface IOrder_StateRepository : IGenericRepositoryAsync<Order_State>
    {
        public Task<List<Order_State>> GetOrder_StatesListAsync();
    }
}
