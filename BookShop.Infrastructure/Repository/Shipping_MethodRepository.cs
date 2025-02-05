using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using BookShop.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repository
{
    public class Shipping_MethodRepository : GenericRepositoryAsync<Shipping_Methods>, IShipping_MethodRepository
    {
        #region Fields
        private readonly DbSet<Shipping_Methods> _shipping_Method;
        #endregion

        #region Contractors
        public Shipping_MethodRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _shipping_Method = dbContext.Set<Shipping_Methods>();
        }
        #endregion

        #region Handle Functions
        public async Task<List<Shipping_Methods>> GetShipping_MethodsListAsync()
        {
            return await _shipping_Method.ToListAsync();
        }
        #endregion
    }
}
