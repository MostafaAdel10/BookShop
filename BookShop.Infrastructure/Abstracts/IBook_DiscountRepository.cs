using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.InfrastructureBases;

namespace BookShop.Infrastructure.Abstracts
{
    public interface IBook_DiscountRepository : IGenericRepositoryAsync<Book_Discount>
    {
    }
}
