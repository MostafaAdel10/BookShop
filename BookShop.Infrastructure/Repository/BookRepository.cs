using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Infrastructure.Repository
{
    public class BookRepository : IBookRepository
    {
        #region Fields
        private readonly ApplicationDbContext _dbContext;
        #endregion

        #region Contractors
        public BookRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Handle Functions
        public async Task<List<Book>> GetBooksListAsync()
        {
            return await _dbContext.Books.Include(s => s.Subject).Include(sub => sub.SubSubject).ToListAsync();
        }
        #endregion


    }
}
