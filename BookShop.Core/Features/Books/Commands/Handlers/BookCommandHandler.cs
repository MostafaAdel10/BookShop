using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Books.Commands.Models;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using BookShop.Service.AuthServices.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Books.Commands.Handlers
{
    public class BookCommandHandler : ResponseHandler,
                        IRequestHandler<AddBookCommand, Response<BookCommand>>,
                        IRequestHandler<AddImagesCommand, Response<string>>,
                        IRequestHandler<EditBookCommand, Response<BookCommand>>,
                        IRequestHandler<DeleteBookCommand, Response<string>>,
                        IRequestHandler<DeleteImageFromBookCommand, Response<string>>,
                        IRequestHandler<DeleteDiscountFromBooksCommand, Response<string>>
    {
        #region Fields
        private readonly IBookService _bookService;
        private readonly ICartItemService _cartItemService;
        private readonly IReviewService _reviewService;
        private readonly IDiscountService _discountService;
        private readonly IBook_DiscountService _book_DiscountService;
        private readonly IBook_ImageService _book_ImageService;
        private readonly IFileService _fileService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public BookCommandHandler(IBookService bookService, IMapper mapper, ICartItemService cartItemService,
            IReviewService reviewService, IFileService fileService, ICurrentUserService currentUserService,
        IStringLocalizer<SharedResources> stringLocalizer, IBook_ImageService book_ImageService,
            IBook_DiscountService book_DiscountService,
        IDiscountService discountService) : base(stringLocalizer)
        {
            _bookService = bookService;
            _book_DiscountService = book_DiscountService;
            _cartItemService = cartItemService;
            _reviewService = reviewService;
            _mapper = mapper;
            _localizer = stringLocalizer;
            _discountService = discountService;
            _book_ImageService = book_ImageService;
            _fileService = fileService;
            _currentUserService = currentUserService;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<BookCommand>> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            // 1 Get the current user ID
            var currentUserId = _currentUserService.GetUserId();

            #region for price after discount
            decimal discountPrice = 0;
            if (request.Discounts is not null && request.Discounts.Count() > 0)
            {
                foreach (int discountId in request.Discounts)
                {
                    var getDiscount = await _discountService.GetDiscountByIdAsync(discountId);
                    if (!getDiscount.IsActive) return UnprocessableEntity<BookCommand>(_localizer[SharedResourcesKeys.DiscountNotActive]);
                    if (getDiscount != null)
                    {
                        var percintage = (await _discountService.GetDiscountByIdAsync(discountId)).Percentage;
                        discountPrice += (percintage / 100) * request.Price;
                    }
                    else
                    {
                        return NotFound<BookCommand>(_localizer[SharedResourcesKeys.DiscountNotExist]);
                    }
                }
            }
            #endregion

            Book book = new()
            {
                Title = request.Title,
                Description = request.Description,
                ISBN13 = request.ISBN13,
                ISBN10 = request.ISBN10,
                Author = request.Author,
                Price = request.Price,
                Publisher = request.Publisher,
                PublicationDate = request.PublicationDate,
                Unit_Instock = request.Unit_Instock,
                SubjectId = request.SubjectId,
                SubSubjectId = request.SubSubjectId,
                IsActive = request.IsActive,
                Image_url = string.Empty,
                PriceAfterDiscount = request.Price - discountPrice,
                CreatedBy = currentUserId
            };

            if (request.ImageData != null)
            {
                var uploadResult = await _fileService.UploadImageAsync(request.ImageData, "Books");
                if (uploadResult == null)
                {
                    return BadRequest<BookCommand>(_localizer[SharedResourcesKeys.FailedToUploadImage]);
                }
                book.Image_url = uploadResult;
            }

            //Add book
            var createdBook = await _bookService.AddAsyncReturnId(book);

            //Adding discounts to the book
            if (request.Discounts != null && request.Discounts.Count() > 0)
            {

                foreach (int dis in request.Discounts)
                {
                    Book_Discount book_discount = new()
                    {
                        DiscountId = dis,
                        BookId = createdBook.Id
                    };
                    await _book_DiscountService.AddBookDiscountAsync(book_discount);
                }
            }
            //Mapping between request and book
            var bookMapper = new BookCommand(createdBook);

            if (bookMapper != null)
                return Created(bookMapper);
            else
                return BadRequest<BookCommand>();
        }

        public async Task<Response<string>> Handle(AddImagesCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookService.GetByIdAsync(request.BookId);
            //Return NotFound
            if (book == null) return NotFound<string>();

            if (request.Images == null || request.Images.Count == 0)
                return BadRequest<string>();

            try
            {
                var imageUrls = await _fileService.UploadImagesAsync(request.Images, "Books");

                var bookImages = imageUrls.Select(url => new Book_Image
                {
                    Image_url = url,
                    BookId = book.Id
                }).ToList();

                await _book_ImageService.AddRangeAsync(bookImages);

                return Created("");
            }
            catch (Exception)
            {
                return BadRequest<string>(_localizer[SharedResourcesKeys.FailedToUploadImage]);
            }
        }

        public async Task<Response<BookCommand>> Handle(EditBookCommand request, CancellationToken cancellationToken)
        {
            // 1 Get the current user ID
            var currentUserId = _currentUserService.GetUserId();

            //Check if the id is exist or not
            var book = await _bookService.GetBookByIdWithIncludeAsync(request.Id);
            //Return NotFound
            if (book == null) return NotFound<BookCommand>();
            //Image
            #region Image 
            if (request.ImageData != null)
            {
                var imageUrl = await _fileService.UpdateImageAsync(book.Image_url, request.ImageData, "Books");
                if (imageUrl == null)
                    return BadRequest<BookCommand>(_localizer[SharedResourcesKeys.FailedToUploadImage]);
                book.Image_url = imageUrl;
            }
            #endregion

            //Discount
            #region Discount
            decimal discountPrice = 0;
            var existingDiscounts = book.Discount != null ? book.Discount.ToList() : new List<Book_Discount>();

            foreach (var existingDiscount in existingDiscounts)
            {
                await _book_DiscountService.DeleteBookDiscount(existingDiscount);
            }
            if (request.Discounts != null)
            {
                foreach (var discountId in request.Discounts)
                {
                    var getDiscount = await _discountService.GetDiscountByIdAsync(discountId);

                    if (!getDiscount.IsActive) return UnprocessableEntity<BookCommand>(_localizer[SharedResourcesKeys.DiscountNotActive]);

                    if (getDiscount != null)
                    {
                        var percintage = (await _discountService.GetDiscountByIdAsync(discountId)).Percentage;
                        discountPrice += (percintage / 100) * request.Price ?? 0;

                        var newDiscount = new Book_Discount
                        {
                            BookId = book.Id,
                            DiscountId = discountId
                        };
                        await _book_DiscountService.AddBookDiscountAsync(newDiscount);
                    }
                    else
                    {
                        return NotFound<BookCommand>(_localizer[SharedResourcesKeys.DiscountNotExist]);
                    }

                }
            }//1000
            #endregion

            //Mapping
            #region Mapping
            book.Title = request.Title!;
            book.Description = request.Description!;
            book.ISBN13 = request.ISBN13;
            book.ISBN10 = request.ISBN10;
            book.Author = request.Author;
            book.Price = request.Price ?? 0m;
            book.PriceAfterDiscount = request.Price - discountPrice ?? 0;
            book.Publisher = request.Publisher;
            book.PublicationDate = request.PublicationDate;
            book.Unit_Instock = request.Unit_Instock;
            book.IsActive = request.IsActive;
            book.SubjectId = request.SubjectId;
            book.SubSubjectId = request.SubSubjectId;
            book.Updated_at = DateTime.Now;
            book.Updated_By = currentUserId;
            #endregion

            //Call service that make edit
            var result = await _bookService.EditAsync(book);
            //Return response
            if (result == "Success")
            {
                //return
                var returnBook = new BookCommand(book);
                return Success(returnBook, _localizer[SharedResourcesKeys.Updated]);
            }
            else
                return BadRequest<BookCommand>();
        }

        public async Task<Response<string>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var book = await _bookService.GetByIdAsync(request.Id);
            //Return NotFound
            if (book == null) return NotFound<string>();

            //Check if book related with order item or not
            var related_order = await _cartItemService.IsBookRelatedWithCartItem(book.Id);
            //Return Related with order item
            if (related_order == true) return UnprocessableEntity<string>(_localizer[SharedResourcesKeys.ReferencedInAnotherTable]);

            //Check if book related with review or not
            var related_review = await _reviewService.IsBookRelatedWithReview(book.Id);
            //Return Related with review
            if (related_review == true) return UnprocessableEntity<string>(_localizer[SharedResourcesKeys.ReferencedInAnotherTable]);

            //delete image
            if (!string.IsNullOrEmpty(book.Image_url))
            {
                var isDeleted = await _fileService.DeleteImageAsync(book.Image_url);
                if (!isDeleted)
                {
                    return BadRequest<string>(_localizer[SharedResourcesKeys.DeletedFailed]);
                }
            }
            //Call service that make delete images related with this book
            var deleteImagesRelatedWithThisBook = await _book_ImageService.GetBook_ImagesByBookIdAsync(book.Id);
            if (deleteImagesRelatedWithThisBook != null)
            {
                foreach (var image_book in deleteImagesRelatedWithThisBook)
                {
                    await _fileService.DeleteImageAsync(image_book.Image_url);
                    await _book_ImageService.DeleteAsync(image_book);
                }
            }
            //Call service that make delete book_discount
            var deleteDiscountsRelatedWithThisBook = await _book_DiscountService.GetBook_DiscountsByBookIdAsync(book.Id);
            if (deleteDiscountsRelatedWithThisBook != null)
            {
                foreach (var discount_book in deleteDiscountsRelatedWithThisBook)
                {
                    await _book_DiscountService.DeleteBookDiscount(discount_book);
                }
            }
            //Call service that make delete book
            var result = await _bookService.DeleteAsync(book);
            //Return response
            if (result == "Success")
                return Deleted<string>();
            else
                return BadRequest<string>();

        }

        public async Task<Response<string>> Handle(DeleteImageFromBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookService.GetBookByIdWithIncludeAsync(request.BookId);
            if (book == null) return NotFound<string>();

            if (book != null)
            {
                //var imageToRemove1 = book.Images != null ? book.Images.FirstOrDefault(img => img.Image_url == request.ImageUrl) : null;
                var imageToRemove = await _book_ImageService.GetImageByBookIdAndImageUrlAsync(request.BookId, request.ImageUrl);
                if (imageToRemove != null)
                {
                    string rootPath = Directory.GetCurrentDirectory();
                    string filePath = Path.Combine(rootPath, "wwwroot", imageToRemove.Image_url.TrimStart('/'));

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    if (book.Images != null)
                        await _book_ImageService.DeleteAsync(imageToRemove);

                    return Deleted<string>();
                }
                else
                    return BadRequest<string>(_localizer[SharedResourcesKeys.NoImage]);
            }
            return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(DeleteDiscountFromBooksCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var discount_book = await _book_DiscountService.GetBook_DiscountsByDiscountIdAsync(request.DiscountId);
            //Return NotFound
            if (discount_book == null || discount_book.Count <= 0) return NotFound<string>();
            decimal discountPrice = 0;
            if (discount_book != null)
            {
                foreach (var disBook in discount_book)
                {
                    var book = await _bookService.GetByIdAsync(disBook.BookId);

                    var percintage = (await _discountService.GetDiscountByIdAsync(request.DiscountId)).Percentage;
                    discountPrice = (percintage / 100) * book.Price;

                    book.PriceAfterDiscount += discountPrice;

                    await _bookService.EditAsync(book);
                    await _book_DiscountService.DeleteBookDiscount(disBook);
                }
                return Deleted<string>();
            }
            else
                return BadRequest<string>(_localizer[SharedResourcesKeys.BadRequest]);
        }

        #endregion
    }
}
