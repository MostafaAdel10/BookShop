using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using BookShop.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repository
{
    public class DiscountRepository : GenericRepositoryAsync<Discount>, IDiscountRepository
    {
        #region Fields
        private readonly DbSet<Discount> _discount;
        #endregion

        #region Contractors
        public DiscountRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _discount = dbContext.Set<Discount>();
        }
        #endregion

        #region Handle Functions
        public async Task<List<Discount>> GetDiscountsListAsync()
        {
            return await _discount.ToListAsync();
        }
        #endregion
    }
}
