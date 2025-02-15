using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using BookShop.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repository
{
    public class Payment_MethodsRepository : GenericRepositoryAsync<Payment_Methods>, IPayment_MethodsRepository
    {
        #region Fields
        private readonly DbSet<Payment_Methods> _payment_Methods;
        #endregion

        #region Contractors
        public Payment_MethodsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _payment_Methods = dbContext.Set<Payment_Methods>();
        }
        #endregion

        #region Handle Functions
        public async Task<List<Payment_Methods>> GetPayment_MethodsListAsync()
        {
            return await _payment_Methods.ToListAsync();
        }
        #endregion
    }
}
