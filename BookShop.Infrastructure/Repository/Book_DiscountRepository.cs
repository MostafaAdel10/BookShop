using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using BookShop.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repository
{
    public class Book_DiscountRepository : GenericRepositoryAsync<Book_Discount>, IBook_DiscountRepository
    {
        #region Fields
        private readonly DbSet<Book_Discount> _book_Discount;
        #endregion

        #region Contractors
        public Book_DiscountRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _book_Discount = dbContext.Set<Book_Discount>();
        }
        #endregion

        #region Handle Functions
        #endregion
    }
}
