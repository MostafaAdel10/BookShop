using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Service.Implementations
{
    internal class Book_DiscountService : IBook_DiscountService
    {
        #region Fields
        private readonly IBook_DiscountRepository _book_DiscountRepository;
        #endregion

        #region Contractors
        public Book_DiscountService(IBook_DiscountRepository book_DiscountRepository)
        {
            _book_DiscountRepository = book_DiscountRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<string> AddBookDiscountAsync(Book_Discount bookDiscount)
        {
            //Added Book_Discount
            await _book_DiscountRepository.AddAsync(bookDiscount);
            return "Success";
        }
        public async Task<string> DeleteBookDiscountAsync(Book_Discount bookDiscount)
        {
            var transaction = _book_DiscountRepository.BeginTransaction();

            try
            {
                await _book_DiscountRepository.DeleteAsync(bookDiscount);
                await transaction.CommitAsync();
                return "Success";
            }
            catch
            {
                await transaction.RollbackAsync();
                return "Failed";
            }
        }

        public async Task<List<Book_Discount>> GetBook_DiscountsByBookIdAsync(int bookId)
        {
            return await _book_DiscountRepository.GetTableAsTracking().Where(x => x.BookId == bookId).ToListAsync();
        }

        public async Task<List<Book_Discount>> GetBook_DiscountsByDiscountIdAsync(int discountId)
        {
            return await _book_DiscountRepository.GetTableAsTracking().Where(x => x.DiscountId == discountId).ToListAsync();
        }

        public async Task<bool> IsDiscountRelatedWithBook(int discountId)
        {
            return await _book_DiscountRepository.GetTableNoTracking().AnyAsync(d => d.DiscountId.Equals(discountId));
        }
        #endregion
    }
}
