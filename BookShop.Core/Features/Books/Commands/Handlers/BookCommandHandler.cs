using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Books.Commands.Models;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Books.Commands.Handlers
{
    public class BookCommandHandler : ResponseHandler,
                        IRequestHandler<AddBookCommand, Response<AddBookCommand>>,
                        IRequestHandler<AddImagesCommand, Response<AddImagesCommand>>,
                        IRequestHandler<EditBookCommand, Response<EditBookCommand>>,
                        IRequestHandler<EditUnit_InstockOfBookCommand, Response<string>>,
                        IRequestHandler<DeleteBookCommand, Response<string>>,
                        IRequestHandler<DeleteImageFromBookCommand, Response<string>>
    {
        #region Fields
        private readonly IBookService _bookService;
        private readonly IDiscountService _discountService;
        private readonly IBook_DiscountService _book_DiscountService;
        private readonly IBook_ImageService _book_ImageService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public BookCommandHandler(IBookService bookService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer, IBook_ImageService book_ImageService,
            IDiscountService discountService) : base(stringLocalizer)
        {
            _bookService = bookService;
            _mapper = mapper;
            _localizer = stringLocalizer;
            _discountService = discountService;
            _book_ImageService = book_ImageService;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<AddBookCommand>> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            #region for price after discount
            decimal discountPrice = 0;
            if (request.Discounts is not null && request.Discounts.Count() > 0)
            {
                foreach (int discountId in request.Discounts)
                {
                    var percintage = (await _discountService.GetDiscountByIdAsync(discountId)).Percentage;
                    discountPrice += (percintage / 100) * request.Price;
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
                IsActive = true,
                Image_url = string.Empty,
                PriceAfterDiscount = request.Price - discountPrice,
                CreatedBy = request.CreatedBy
            };
            if (request.ImageData != null)
            {
                var fileName = Guid.NewGuid() + "_" + request.ImageData.FileName;

                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/books");

                Directory.CreateDirectory(uploadFolder);
                string filePath = Path.Combine(uploadFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageData.CopyToAsync(fileStream);
                }

                book.Image_url = "/images/books/" + fileName;
            }

            //Add book
            var createdBook = await _bookService.AddAsyncReturnId(book);

            //Adding discounts to the book
            if (request.Discounts is not null && request.Discounts.Count() > 0)
            {

                foreach (int dis in request.Discounts)
                {
                    await _book_DiscountService.AddBookDiscountAsync(new Book_Discount { BookId = createdBook.Id, DiscountId = dis });
                }
            }
            //Mapping between request and book
            var bookMapper = _mapper.Map<AddBookCommand>(createdBook);

            if (bookMapper != null)
                return Created(bookMapper);
            else
                return BadRequest<AddBookCommand>();
        }

        public async Task<Response<AddImagesCommand>> Handle(AddImagesCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookService.GetByIdAsync(request.Id);
            //Return NotFound
            if (book == null) return NotFound<AddImagesCommand>();

            if (request.Images != null && request.Images.Count != 0)
            {
                var bookImages = new List<Book_Image>();

                foreach (var imageFile in request.Images)
                {
                    var fileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;

                    string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/books");

                    Directory.CreateDirectory(uploadFolder);

                    string filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    var bookImage = new Book_Image
                    {
                        Image_url = $"/images/books/{fileName}",
                        BookId = book.Id,
                        Books = book
                    };

                    bookImages.Add(bookImage);

                    foreach (var image in bookImages)
                    {
                        if (book.Images != null)
                            book.Images.Add(image);
                    }
                    await _book_ImageService.SaveChangesAsync();

                    return Created(request);
                }
                return BadRequest<AddImagesCommand>(_localizer[SharedResourcesKeys.FailedToUploadImage]);
            }
            else
                return BadRequest<AddImagesCommand>(_localizer[SharedResourcesKeys.FailedToUploadImage]);
        }

        public async Task<Response<EditBookCommand>> Handle(EditBookCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var book = await _bookService.GetByIdAsync(request.Id);
            //Return NotFound
            if (book == null) return NotFound<EditBookCommand>();
            //Image
            #region Image 
            if (request.ImageD != null)
            {
                if (!string.IsNullOrEmpty(book.Image_url))
                {
                    var oldFilePath = Path.Combine("wwwroot", book.Image_url.TrimStart('/'));
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                var fileName = Guid.NewGuid() + Path.GetExtension(request.ImageD.FileName);
                var filePath = Path.Combine("wwwroot/images/books", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageD.CopyToAsync(stream);
                }

                book.Image_url = $"/images/books/{fileName}";
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
                    var newDiscount = new Book_Discount
                    {
                        BookId = book.Id,
                        DiscountId = discountId
                    };
                    await _book_DiscountService.AddBookDiscountAsync(newDiscount);
                    var percintage = (await _discountService.GetDiscountByIdAsync(discountId)).Percentage;
                    discountPrice += (percintage / 100) * request.Price ?? 0;
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
            book.PriceAfterDiscount = request.Price - discountPrice;
            book.Publisher = request.Publisher;
            book.PublicationDate = request.PublicationDate;
            book.Unit_Instock = request.Unit_Instock ?? 0;
            book.IsActive = request.IsActive;
            book.SubjectId = request.SubjectId;
            book.SubSubjectId = request.SubSubjectId;
            book.Updated_at = DateTime.Now;
            book.Updated_By = request.Updated_By;
            #endregion

            //Call service that make edit
            var result = await _bookService.EditAsync(book);
            //Return response
            if (result == "Success")
            {
                // Map back to DTO and return
                var returnBook = _mapper.Map<EditBookCommand>(result);
                return Success(returnBook, _localizer[SharedResourcesKeys.Updated]);
            }
            else
                return BadRequest<EditBookCommand>();
        }

        public async Task<Response<string>> Handle(EditUnit_InstockOfBookCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var book = await _bookService.GetByIdAsync(request.BookId);
            //Return NotFound
            if (book == null) return NotFound<string>();

            if (!request.IsSubtract)
            {
                book.Unit_Instock = (book.Unit_Instock + request.quantity);
            }
            else
            {
                if ((book.Unit_Instock - request.quantity) >= 0)
                {
                    book.Unit_Instock = (book.Unit_Instock - request.quantity);
                }
            }
            //Call service that make edit
            var result = await _bookService.EditAsync(book);
            //Return response
            if (result == "Success")
            {
                return Success<string>(_localizer[SharedResourcesKeys.Updated]);
            }
            else
                return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var book = await _bookService.GetByIdAsync(request.Id);
            //Return NotFound
            if (book == null) return NotFound<string>();
            //delete image
            if (!string.IsNullOrEmpty(book.Image_url))
            {
                var oldFilePath = Path.Combine("wwwroot", book.Image_url.TrimStart('/'));
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }
            //Call service that make delete
            var result = await _bookService.DeleteAsync(book);
            //Return response
            if (result == "Success")
                return Deleted<string>();
            else
                return BadRequest<string>();

        }

        public async Task<Response<string>> Handle(DeleteImageFromBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookService.GetByIdAsync(request.BookId);
            if (book != null)
            {
                var imageToRemove = book.Images != null ? book.Images.FirstOrDefault(img => img.Image_url == request.ImageUrl) : null;
                if (imageToRemove != null)
                {
                    string rootPath = Directory.GetCurrentDirectory();
                    string filePath = Path.Combine(rootPath, "wwwroot", imageToRemove.Image_url.TrimStart('/'));

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    if (book.Images != null)
                        book.Images.Remove(imageToRemove);

                    await _book_ImageService.SaveChangesAsync();
                    return Deleted<string>();
                }
                else
                    return BadRequest<string>(_localizer[SharedResourcesKeys.NoImage]);
            }
            return BadRequest<string>();
        }

        #endregion
    }
}
