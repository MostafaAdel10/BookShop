using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.InfrastructureBases;

namespace BookShop.Infrastructure.Abstracts
{
    public interface IOrderItemRepository : IGenericRepositoryAsync<OrderItem>
    {
    }
}
