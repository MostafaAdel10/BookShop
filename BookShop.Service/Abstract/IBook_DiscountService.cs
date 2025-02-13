using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface IBook_DiscountService
    {
        public Task<Book_Discount> AddBookDiscountAsync(Book_Discount bookDiscount);
        public Task<string> DeleteBookDiscount(Book_Discount bookDiscount);
    }
}
