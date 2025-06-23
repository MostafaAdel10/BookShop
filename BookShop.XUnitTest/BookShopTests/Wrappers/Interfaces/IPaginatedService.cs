using BookShop.Core.Wrappers;

namespace BookShop.XUnitTest.BookShopTests.Wrappers.Interfaces
{
    public interface IPaginatedService<T>
    {
        public Task<PaginatedResult<T>> ReturnPaginatedResult(IQueryable<T> source, int pageNumber, int pageSize);
    }
}
