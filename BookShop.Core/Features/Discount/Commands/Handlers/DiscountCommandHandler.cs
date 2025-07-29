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
            if (request.ImageData != null)
            {
                var imageUrl = await _fileService.UploadImageAsync(request.ImageData, "Discounts");
                if (imageUrl == null)
                {
                    return BadRequest<DiscountCommand>(_localizer[SharedResourcesKeys.FailedToUploadImage]);
                }
                request.ImageUrl = imageUrl;
            }

            //Mapping between request and discount
            var discountMapper = _mapper.Map<DataAccess.Entities.Discount>(request);

            //Add
            var result = await _discountService.AddAsync(discountMapper);

            if (result == "Success")
            {
                // Map back to DTO and return
                var returnDiscount = _mapper.Map<DiscountCommand>(discountMapper);
                return Created(returnDiscount);
            }
            else
                return BadRequest<DiscountCommand>();
        }

        public async Task<Response<DiscountCommand>> Handle(EditDiscountCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var discount = await _discountService.GetDiscountByIdAsync(request.Id);
            //Return NotFound
            if (discount == null) return NotFound<DiscountCommand>();
            // Add Image
            if (request.ImageData != null)
            {
                var imageUrl = await _fileService.UpdateImageAsync(discount.ImageUrl, request.ImageData, "Discounts");
                if (imageUrl == null)
                {
                    return BadRequest<DiscountCommand>(_localizer[SharedResourcesKeys.FailedToUploadImage]);
                }
                request.ImageUrl = imageUrl;
            }
            //Mapping between request and discounts
            var discountMapper = _mapper.Map(request, discount);
            //Call service that make edit
            var result = await _discountService.EditAsync(discountMapper);
            //Return response
            if (result == "Success")
            {
                // Map back to DTO and return
                var returnDiscount = _mapper.Map<DiscountCommand>(discountMapper);
                return Success(returnDiscount, _localizer[SharedResourcesKeys.Updated]);
            }
            else
                return BadRequest<DiscountCommand>();
        }

        public async Task<Response<string>> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var discount = await _discountService.GetDiscountByIdAsync(request.Id);
            //Return NotFound
            if (discount == null) return NotFound<string>();
            //Check if discount related with book or not
            var related_discount = await _book_discountService.IsDiscountRelatedWithBook(discount.Id);
            //Return Related with book
            if (related_discount == true) return UnprocessableEntity<string>(_localizer[SharedResourcesKeys.ReferencedInAnotherTable]);
            //Delete Image
            if (!string.IsNullOrEmpty(discount.ImageUrl))
            {
                var isDeleted = _fileService.DeleteImage(discount.ImageUrl);
                if (!isDeleted)
                {
                    return BadRequest<string>(_localizer[SharedResourcesKeys.DeletedFailed]);
                }
            }
            //Call service that make delete
            var result = await _discountService.DeleteAsync(discount);
            //Return response
            if (result == "Success")
                return Deleted<string>();
            else
                return BadRequest<string>();
        }
        #endregion
    }
}
