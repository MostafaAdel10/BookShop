using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using BookShop.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Infrastructure.Repository
{
    public class Cart_TypeRepository : GenericRepositoryAsync<Card_Type>, ICart_TypeRepository
    {
        #region Fields
        private readonly DbSet<Card_Type> _card_Type;
        #endregion

        #region Contractors
        public Cart_TypeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _card_Type = dbContext.Set<Card_Type>();
        }
        #endregion

        #region Handle Functions
        public async Task<List<Card_Type>> GetCard_TypesListAsync()
        {
            return await _card_Type.ToListAsync();
        }
        #endregion
    }
}
