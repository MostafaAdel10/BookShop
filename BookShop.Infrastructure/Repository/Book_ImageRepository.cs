using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using BookShop.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repository
{
    public class Book_ImageRepository : GenericRepositoryAsync<Book_Image>, IBook_ImageRepository
    {
        #region Fields
        private readonly DbSet<Book_Image> _book_Image;
        #endregion

        #region Contractors
        public Book_ImageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _book_Image = dbContext.Set<Book_Image>();
        }
        #endregion

        #region Handle Functions
        #endregion
    }
}
