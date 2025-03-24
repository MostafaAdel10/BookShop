using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using BookShop.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repository
{
    public class AddressRepository : GenericRepositoryAsync<Address>, IAddressRepository
    {
        #region Fields
        private readonly DbSet<Address> _address;
        #endregion

        #region Contractors
        public AddressRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _address = dbContext.Set<Address>();
        }
        #endregion

        #region Handle Functions
        #endregion
    }
}
