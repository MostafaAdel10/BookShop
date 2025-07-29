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

            if (request.Discounts is not null && request.Discounts.Any())
            {
                foreach (int discountId in request.Discounts)
                {
                    var discount = await _discountService.GetDiscountByIdAsync(discountId);

                    if (discount is null)
                    {
                        return NotFound<BookCommand>(_localizer[SharedResourcesKeys.DiscountNotExist]);
                    }

                    if (!discount.IsActive)
                    {
                        return UnprocessableEntity<BookCommand>(_localizer[SharedResourcesKeys.DiscountNotActive]);
                    }

                    discountPrice += (discount.Percentage / 100m) * request.Price;
                }
            }
            #endregion

            //Fill Book Object
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
            if (request.Discounts?.Any() == true)
            {
                foreach (var discountId in request.Discounts)
                {
                    var bookDiscount = new Book_Discount
                    {
                        DiscountId = discountId,
                        BookId = createdBook.Id
                    };

                    await _book_DiscountService.AddBookDiscountAsync(bookDiscount);
                }
            }

            //Mapping between request and book
            var bookMapper = new BookCommand(createdBook);

            //Return Response
            if (bookMapper != null)
                return Created(bookMapper);
            else
                return BadRequest<BookCommand>();
        }

        public async Task<Response<string>> Handle(AddImagesCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookService.GetByIdAsync(request.BookId);

            if (book is null)
                return NotFound<string>();

            if (request.Images is null || !request.Images.Any())
                return BadRequest<string>(_localizer[SharedResourcesKeys.NoImagesProvided]);

            try
            {
                var imageUrls = await _fileService.UploadImagesAsync(request.Images, "Books");

                var bookImages = imageUrls.Select(url => new Book_Image
                {
                    Image_url = url,
                    BookId = book.Id
                }).ToList();

                await _book_ImageService.AddRangeAsync(bookImages);

                return Created<string>(_localizer[SharedResourcesKeys.ImagesUploadedSuccessfully]);
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
            if (book is null)
                return NotFound<BookCommand>();

            // Handle Image
            #region Image
            if (request.ImageData is not null)
            {
                var imageUrl = await _fileService.UpdateImageAsync(book.Image_url, request.ImageData, "Books");
                if (imageUrl is null)
                    return BadRequest<BookCommand>(_localizer[SharedResourcesKeys.FailedToUploadImage]);

                book.Image_url = imageUrl;
            }
            #endregion

            // Handle Discounts
            #region Discount

            decimal discountPrice = 0;
            var existingDiscounts = book.Discount?.ToList() ?? new List<Book_Discount>();
            foreach (var existing in existingDiscounts)
                await _book_DiscountService.DeleteBookDiscountAsync(existing);

            if (request.Discounts is not null)
            {
                foreach (var discountId in request.Discounts)
                {
                    var discount = await _discountService.GetDiscountByIdAsync(discountId);
                    if (discount is null)
                        return NotFound<BookCommand>(_localizer[SharedResourcesKeys.DiscountNotExist]);

                    if (!discount.IsActive)
                        return UnprocessableEntity<BookCommand>(_localizer[SharedResourcesKeys.DiscountNotActive]);

                    discountPrice += (discount.Percentage / 100m) * (request.Price ?? 0);

                    await _book_DiscountService.AddBookDiscountAsync(new Book_Discount
                    {
                        BookId = book.Id,
                        DiscountId = discountId
                    });
                }
            }

            #endregion

            // Mapping updated data
            #region Mapping

            book.Title = request.Title!;
            book.Description = request.Description!;
            book.ISBN13 = request.ISBN13;
            book.ISBN10 = request.ISBN10;
            book.Author = request.Author;
            book.Price = request.Price ?? 0m;
            book.PriceAfterDiscount = (request.Price ?? 0m) - discountPrice;
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
            return result == "Success"
                ? Success(new BookCommand(book), _localizer[SharedResourcesKeys.Updated])
                : BadRequest<BookCommand>();
        }

        public async Task<Response<string>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            // Step 1: Check if the book exists
            var book = await _bookService.GetByIdAsync(request.Id);
            if (book is null)
                return NotFound<string>();

            // Step 2: Validate book is not related to cart items or reviews
            if (await _cartItemService.IsBookRelatedWithCartItem(book.Id))
                return UnprocessableEntity<string>(_localizer[SharedResourcesKeys.ReferencedInAnotherTable]);

            if (await _reviewService.IsBookRelatedWithReview(book.Id))
                return UnprocessableEntity<string>(_localizer[SharedResourcesKeys.ReferencedInAnotherTable]);

            // Step 3: Delete main image if exists
            if (!string.IsNullOrEmpty(book.Image_url))
            {
                var isDeleted = _fileService.DeleteImage(book.Image_url);
                if (!isDeleted)
                    return BadRequest<string>(_localizer[SharedResourcesKeys.DeletedFailed]);
            }

            // Step 4: Delete related images
            var relatedImages = await _book_ImageService.GetBook_ImagesByBookIdAsync(book.Id);
            if (relatedImages?.Any() == true)
            {
                foreach (var image in relatedImages)
                {
                    _fileService.DeleteImage(image.Image_url);
                    await _book_ImageService.DeleteAsync(image);
                }
            }

            // Step 5: Delete related discounts
            var relatedDiscounts = await _book_DiscountService.GetBook_DiscountsByBookIdAsync(book.Id);
            if (relatedDiscounts?.Any() == true)
            {
                foreach (var discount in relatedDiscounts)
                {
                    await _book_DiscountService.DeleteBookDiscountAsync(discount);
                }
            }

            // Step 6: Delete the book itself
            var result = await _bookService.DeleteAsync(book);

            //Return response
            return result == "Success"
                ? Deleted<string>()
                : BadRequest<string>();
        }

        public async Task<Response<string>> Handle(DeleteImageFromBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookService.GetBookByIdWithIncludeAsync(request.BookId);
            if (book is null)
                return NotFound<string>();

            var imageToRemove = await _book_ImageService.GetImageByBookIdAndImageUrlAsync(request.BookId, request.ImageUrl);
            if (imageToRemove is null)
                return NotFound<string>(_localizer[SharedResourcesKeys.NoImage]);

            var isDeleted = _fileService.DeleteImage(imageToRemove.Image_url);
            if (!isDeleted)
                return BadRequest<string>(_localizer[SharedResourcesKeys.DeletedFailed]);

            await _book_ImageService.DeleteAsync(imageToRemove);

            return Deleted<string>();

        }

        public async Task<Response<string>> Handle(DeleteDiscountFromBooksCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var discountBooks = await _book_DiscountService.GetBook_DiscountsByDiscountIdAsync(request.DiscountId);

            //Return NotFound
            if (discountBooks == null || !discountBooks.Any())
                return NotFound<string>();

            var discount = await _discountService.GetDiscountByIdAsync(request.DiscountId);
            if (discount == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.DiscountNotExist]);

            foreach (var disBook in discountBooks)
            {
                var book = await _bookService.GetByIdAsync(disBook.BookId);
                if (book is null)
                    continue; // or log warning

                var discountAmount = (discount.Percentage / 100m) * book.Price;

                book.PriceAfterDiscount = book.PriceAfterDiscount + discountAmount;

                await _bookService.EditAsync(book);
                await _book_DiscountService.DeleteBookDiscountAsync(disBook);
            }

            return Deleted<string>();
        }

        #endregion
    }
}
