using BookShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Service.Abstract
{
    public interface IBookService
    {
        public Task<List<Book>> GetBooksListAsync();
    }
}
