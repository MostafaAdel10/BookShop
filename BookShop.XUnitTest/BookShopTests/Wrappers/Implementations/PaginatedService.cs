using BookShop.Core.Wrappers;
using BookShop.DataAccess.Entities;
using BookShop.XUnitTest.BookShopTests.Wrappers.Interfaces;

namespace BookShop.XUnitTest.BookShopTests.Wrappers.Implementations
{
    public class PaginatedService : IPaginatedService<Book>
    {
        public async Task<PaginatedResult<Book>> ReturnPaginatedResult(IQueryable<Book> source, int pageNumber, int pageSize)
        {
            return await source.ToPaginatedListAsync(pageNumber, pageSize);
        }
    }
}
