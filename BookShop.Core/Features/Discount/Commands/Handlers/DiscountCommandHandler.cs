using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Discount.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Discount.Commands.Handlers
{
    public class DiscountCommandHandler : ResponseHandler,
                        IRequestHandler<AddDiscountCommand, Response<DiscountCommand>>,
                        IRequestHandler<EditDiscountCommand, Response<DiscountCommand>>,
                        IRequestHandler<DeleteDiscountCommand, Response<string>>
    {
        #region Fields
        private readonly IDiscountService _discountService;
        private readonly IBook_DiscountService _book_discountService;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public DiscountCommandHandler(IDiscountService discountService, IMapper mapper,
            IBook_DiscountService book_discountService, IFileService fileService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _discountService = discountService;
            _book_discountService = book_discountService;
            _fileService = fileService;
            _mapper = mapper;
            _localizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<DiscountCommand>> Handle(AddDiscountCommand request, CancellationToken cancellationToken)
        {
            // Add Image
            string? imageUrl = null;
            if (request.ImageData is not null)
            {
                imageUrl = await _fileService.UploadImageAsync(request.ImageData, "Discounts");
                if (string.IsNullOrWhiteSpace(imageUrl))
                    return BadRequest<DiscountCommand>(_localizer[SharedResourcesKeys.FailedToUploadImage]);
            }

            //Mapping between request and discount
            var discount = new DataAccess.Entities.Discount
            {
                Name = request.Name,
                Name_Ar = request.Name_Ar,
                ImageUrl = imageUrl,
                Code = request.Code,
                Start_date = request.Start_date,
                End_date = request.End_date,
                IsActive = request.IsActive,
                Percentage = request.Percentage
            };
            //Add
            var result = await _discountService.AddAsync(discount);

            return result == "Success"
            ? Created(_mapper.Map<DiscountCommand>(discount))
            : BadRequest<DiscountCommand>(_localizer[SharedResourcesKeys.FailedToAdd]);
        }

        public async Task<Response<DiscountCommand>> Handle(EditDiscountCommand request, CancellationToken cancellationToken)
        {
            var discount = await _discountService.GetDiscountByIdAsync(request.Id);
            if (discount is null)
                return NotFound<DiscountCommand>();

            // Add Image
            if (request.ImageData is not null)
            {
                var imageUrl = await _fileService.UpdateImageAsync(discount.ImageUrl, request.ImageData, "Discounts");
                if (string.IsNullOrWhiteSpace(imageUrl))
                    return BadRequest<DiscountCommand>(_localizer[SharedResourcesKeys.FailedToUploadImage]);

                discount.ImageUrl = imageUrl; // تعيين URL الصورة الجديد
            }

            //Mapping between request and discounts
            _mapper.Map(request, discount); // تعديل نفس الكائن

            //Call service that make edit
            var result = await _discountService.EditAsync(discount);

            //Return response
            return result == "Success"
            ? Success(_mapper.Map<DiscountCommand>(discount), _localizer[SharedResourcesKeys.Updated])
            : BadRequest<DiscountCommand>(_localizer[SharedResourcesKeys.FailedToUpdate]);
        }

        public async Task<Response<string>> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            var discount = await _discountService.GetDiscountByIdAsync(request.Id);
            if (discount is null)
                return NotFound<string>();

            //Check if discount related with book or not
            var isRelated = await _book_discountService.IsDiscountRelatedWithBook(discount.Id);
            if (isRelated)
                return UnprocessableEntity<string>(_localizer[SharedResourcesKeys.ReferencedInAnotherTable]);

            //Delete Image
            if (!string.IsNullOrWhiteSpace(discount.ImageUrl))
            {
                var isDeleted = _fileService.DeleteImage(discount.ImageUrl);
                if (!isDeleted)
                    return BadRequest<string>(_localizer[SharedResourcesKeys.DeletedFailed]);
            }

            //Call service that make delete
            var result = await _discountService.DeleteAsync(discount);

            //Return response
            return result == "Success"
            ? Deleted<string>()
            : BadRequest<string>(_localizer[SharedResourcesKeys.FailedToDelete]);
        }
        #endregion
    }
}
