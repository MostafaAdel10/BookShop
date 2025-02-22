using BookShop.DataAccess.Entities;
using BookShop.DataAccess.Helpers;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BookShop.Service.Implementations
{
    public class BookService : IBookService
    {
        #region Fields
        private readonly IBookRepository _bookRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IBook_DiscountRepository _book_DiscountRepository;
        private readonly ISubSubjectRepository _subSubjectRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IMemoryCache _memoryCache;
        private string key = "Books";
        #endregion

        #region Contractors
        public BookService(IBookRepository bookRepository, ISubSubjectRepository subSubjectRepository,
            ISubjectRepository subjectRepository, IBook_DiscountRepository book_DiscountRepository,
            IMemoryCache memoryCache, IOrderItemRepository orderItemRepository)
        {
            _bookRepository = bookRepository;
            _subSubjectRepository = subSubjectRepository;
            _subjectRepository = subjectRepository;
            _book_DiscountRepository = book_DiscountRepository;
            _memoryCache = memoryCache;
            _orderItemRepository = orderItemRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<List<Book>> GetBooksListAsync()
        {
            return await _bookRepository.GetBooksListAsync();
        }

        public async Task<Book> GetBookByIdWithIncludeAsync(int id)
        {
            //var book = _bookRepository.GetByIdAsync(id);
            var book = _bookRepository.GetTableNoTracking()
                .Include(s => s.Subject)
                .Include(sub => sub.SubSubject)
                .Where(b => b.Id == id)
                .FirstOrDefault();

            return book;
        }

        public async Task<string> AddAsync(Book book)
        {
            //Added Book
            await _bookRepository.AddAsync(book);
            return "Success";
        }

        public async Task<bool> IsISBNExist(string isbn)
        {
            //Check if the ISBN exists or not
            var book = _bookRepository.GetTableNoTracking().Where(b => b.ISBN13.Equals(isbn)).FirstOrDefault();
            if (book == null) return false;
            return true;
        }

        public async Task<bool> IsISBNExistExcludeSelf(string isbn, int id)
        {
            //Check if the ISBN exists or not
            var book = await _bookRepository.GetTableNoTracking().Where(b => b.ISBN13.Equals(isbn) & !b.Id.Equals(id)).FirstOrDefaultAsync();
            if (book == null) return false;
            return true;
        }

        public async Task<string> EditAsync(Book book)
        {
            await _bookRepository.UpdateAsync(book);
            return "Success";
        }

        public async Task<string> DeleteAsync(Book book)
        {
            var transaction = _bookRepository.BeginTransaction();

            try
            {
                await _bookRepository.DeleteAsync(book);
                await transaction.CommitAsync();
                return "Success";
            }
            catch
            {
                await transaction.RollbackAsync();
                return "Failed";
            }
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            return book;
        }

        public IQueryable<Book> GetBookQueryable()
        {
            return _bookRepository.GetTableNoTracking()
                                  .Include(s => s.Subject)
                                  .Include(sub => sub.SubSubject)
                                  .AsQueryable();
        }

        public IQueryable<Book> FilterBookPaginatedQueryable(BookOrderingEnum orderingEnum, string search)
        {
            var queryable = _bookRepository.GetTableNoTracking()
                                      .Include(s => s.Subject)
                                      .Include(sub => sub.SubSubject)
                                      .AsQueryable();

            if (search != null)
            {
                queryable = queryable.Where(b => b.ISBN13.Contains(search) || b.Title.Contains(search));
            }

            switch (orderingEnum)
            {
                case BookOrderingEnum.Id:
                    queryable = queryable.OrderBy(b => b.Id);
                    break;
                case BookOrderingEnum.Title:
                    queryable = queryable.OrderBy(b => b.Title);
                    break;
                case BookOrderingEnum.Description:
                    queryable = queryable.OrderBy(b => b.Description);
                    break;
                case BookOrderingEnum.ISBN13:
                    queryable = queryable.OrderBy(b => b.ISBN13);
                    break;
                case BookOrderingEnum.Author:
                    queryable = queryable.OrderBy(b => b.Author);
                    break;
                case BookOrderingEnum.Price:
                    queryable = queryable.OrderBy(b => b.Price);
                    break;
                case BookOrderingEnum.PriceAfterDiscount:
                    queryable = queryable.OrderBy(b => b.PriceAfterDiscount);
                    break;
                case BookOrderingEnum.Publisher:
                    queryable = queryable.OrderBy(b => b.Publisher);
                    break;
                case BookOrderingEnum.PublicationDate:
                    queryable = queryable.OrderBy(b => b.PublicationDate);
                    break;
                case BookOrderingEnum.Unit_Instock:
                    queryable = queryable.OrderBy(b => b.Unit_Instock);
                    break;
                case BookOrderingEnum.IsActive:
                    queryable = queryable.OrderBy(b => b.IsActive);
                    break;
                case BookOrderingEnum.SubjectName:
                    queryable = queryable.OrderBy(b => b.Subject.Name);
                    break;
                case BookOrderingEnum.SubSubjectName:
                    queryable = queryable.OrderBy(b => b.SubSubject.Name);
                    break;
                case BookOrderingEnum.ISBN10:
                    queryable = queryable.OrderBy(b => b.ISBN10);
                    break;
                default:
                    queryable = queryable.OrderBy(b => b.Id);
                    break;
            }

            return queryable;
        }

        public IQueryable<Book> GetBookBySubjectIdQueryable(int SID)
        {
            return _bookRepository.GetTableNoTracking()
                                  .Where(x => x.SubjectId.Equals(SID))
                                  .AsQueryable();
        }

        public IQueryable<Book> GetBookBySubSubjectIdQueryable(int SSID)
        {
            return _bookRepository.GetTableNoTracking()
                                  .Where(x => x.SubSubjectId.Equals(SSID))
                                  .AsQueryable();
        }

        public async Task<bool> SubSubjectRelatedWithBook(int id)
        {
            return await _bookRepository.SubSubjectRelatedWithBook(id);
        }

        public async Task<bool> SubjectRelatedWithBook(int id)
        {
            return await _bookRepository.SubjectRelatedWithBook(id);
        }

        public async Task<bool> IsSubjectIdExist(int subjectId)
        {
            //Check if the subjectId is Exist Or not
            var subject = _subjectRepository.GetTableNoTracking().Where(s => s.Id.Equals(subjectId)).FirstOrDefault();
            if (subject == null) return false;
            return true;
        }

        public async Task<bool> IsSubSubjectIdExist(int subSubjectId)
        {
            //Check if the subSubjectId is Exist Or not
            var subject = _subSubjectRepository.GetTableNoTracking().Where(ss => ss.Id.Equals(subSubjectId)).FirstOrDefault();
            if (subject == null) return false;
            return true;
        }

        public async Task<Book> AddAsyncReturnId(Book book)
        {
            //Added Book
            return await _bookRepository.AddAsync(book);
        }

        public async Task<bool> IsBookIdExist(int id)
        {
            //Check if the book exists or not
            var book = _bookRepository.GetTableNoTracking().Where(b => b.Id.Equals(id)).FirstOrDefault();
            if (book == null) return false;
            return true;
        }

        public async Task<string> EditUnit_InstockOfBookCommand(int bookId, int quantity, bool isSubtract = true)
        {
            //Check if the id is exist or not
            var book = await _bookRepository.GetByIdAsync(bookId);
            //Return NotFound
            if (book == null) return "NotFound";

            if (!isSubtract)
            {
                book.Unit_Instock = (book.Unit_Instock + quantity);
            }
            else
            {
                if ((book.Unit_Instock - quantity) >= 0)
                {
                    book.Unit_Instock = (book.Unit_Instock - quantity);
                }
            }
            //Call service that make edit
            await _bookRepository.UpdateAsync(book);
            return "Success";

        }

        public void AddToCache(string key, object value, TimeSpan? absoluteExpiration = null)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpiration
            };

            _memoryCache.Set(key, value, cacheEntryOptions);
        }

        public T GetFromCache<T>(string key)
        {
            _memoryCache.TryGetValue(key, out T value);
            return value;
        }

        public void RemoveFromCache(string key)
        {
            _memoryCache.Remove(key);
        }

        public List<Book> AddtoCashMemoery(string key, List<Book> items)
        {
            var DataIncash = GetFromCache<List<Book>>(key);
            if (DataIncash != null && DataIncash.Count > 0)
            {
                foreach (var item in items)
                {
                    if (!DataIncash.Any(existingItem => existingItem.Id == item.Id))
                    {
                        DataIncash.Insert(0, item);
                    }
                }
                AddToCache(key, DataIncash, TimeSpan.FromMinutes(30));
                return DataIncash;
            }
            else
            {
                AddToCache(key, items, TimeSpan.FromMinutes(30));
                return items;
            }
        }

        public void RemoveFromCashMemoery(string key, Book item)
        {
            var DataIncash = GetFromCache<List<Book>>(key);
            if (DataIncash != null && DataIncash.Count > 0)
            {
                if (DataIncash.Where(existingItem => existingItem.Id == item.Id).ToList().Count == 1)
                {
                    var data = DataIncash.FirstOrDefault(p => p.Id == item.Id);
                    if (data != null)
                    {
                        DataIncash.Remove(data);
                        AddToCache(key, DataIncash, TimeSpan.FromMinutes(30));
                    }
                }
            }
        }

        public async Task<bool> IsQuantityGraterThanExist(int bookId, int quantity)
        {
            //Check if the quantity Grater Than Exist  or not
            var book = _bookRepository.GetTableNoTracking().Where(b => b.Id.Equals(bookId)).FirstOrDefault();

            if (book == null) return false;
            if (book.Unit_Instock < quantity) return false;
            return true;
        }

        public async Task<bool> IsPriceTrueExist(int bookId, decimal price)
        {
            //Check if the Price true or not
            var book = _bookRepository.GetTableNoTracking().Where(b => b.Id.Equals(bookId)).FirstOrDefault();
            if (book == null) return false;
            if (book.Price != price) return false;
            return true;
        }

        public async Task<bool> IsTheBookInStock(int bookId)
        {
            //Check if the book in stock or not
            var book = _bookRepository.GetTableNoTracking().Where(b => b.Id.Equals(bookId)).FirstOrDefault();
            if (book == null) return false;
            if (book.Unit_Instock == 0) return false;
            return true;
        }
        #endregion



    }
}
