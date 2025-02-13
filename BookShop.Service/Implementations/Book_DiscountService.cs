using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;

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
        public async Task<Book_Discount> AddBookDiscountAsync(Book_Discount bookDiscount)
        {
            //Added Book_Discount
            return await _book_DiscountRepository.AddAsync(bookDiscount);
        }
        public async Task<string> DeleteBookDiscount(Book_Discount bookDiscount)
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
        #endregion
    }
}
