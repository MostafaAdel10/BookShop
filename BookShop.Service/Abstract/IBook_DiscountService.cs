using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface IBook_DiscountService
    {
        public Task<List<Book_Discount>> GetBook_DiscountsByBookIdAsync(int bookId);
        public Task<List<Book_Discount>> GetBook_DiscountsByDiscountIdAsync(int discountId);
        public Task<string> AddBookDiscountAsync(Book_Discount bookDiscount);
        public Task<string> DeleteBookDiscount(Book_Discount bookDiscount);
        public Task<bool> IsDiscountRelatedWithBook(int discountId);
    }
}
