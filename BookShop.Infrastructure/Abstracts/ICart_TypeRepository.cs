using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.InfrastructureBases;

namespace BookShop.Infrastructure.Abstracts
{
    public interface ICart_TypeRepository : IGenericRepositoryAsync<Card_Type>
    {
        public Task<List<Card_Type>> GetCard_TypesListAsync();
    }
}
