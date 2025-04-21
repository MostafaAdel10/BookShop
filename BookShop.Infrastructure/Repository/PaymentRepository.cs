using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using BookShop.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repository
{
    public class PaymentRepository : GenericRepositoryAsync<Payment>, IPaymentRepository
    {
        #region Fields
        private readonly DbSet<Payment> _payment;
        #endregion

        #region Contractors
        public PaymentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _payment = dbContext.Set<Payment>();
        }
        #endregion

        #region Handle Functions
        #endregion
    }
}
