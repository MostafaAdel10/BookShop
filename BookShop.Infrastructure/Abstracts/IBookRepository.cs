using BookShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Infrastructure.Abstracts
{
    public interface IBookRepository
    {
        public Task<List<Book>> GetBooksListAsync();
    }
}
