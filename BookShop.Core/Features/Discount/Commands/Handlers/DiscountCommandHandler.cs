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
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public DiscountCommandHandler(IDiscountService discountService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _discountService = discountService;
            _mapper = mapper;
            _localizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<DiscountCommand>> Handle(AddDiscountCommand request, CancellationToken cancellationToken)
        {
            //
            if (request.ImageData != null)
            {
                var fileName = Guid.NewGuid() + "_" + request.ImageData.FileName;

                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Discounts");

                Directory.CreateDirectory(uploadFolder);
                string filePath = Path.Combine(uploadFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageData.CopyToAsync(fileStream);
                }

                request.ImageUrl = "/images/Discounts/" + fileName;
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
            //
            if (request.ImageData != null)
            {
                if (!string.IsNullOrEmpty(discount.ImageUrl))
                {
                    var oldFilePath = Path.Combine("wwwroot", discount.ImageUrl.TrimStart('/'));
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                var fileName = Guid.NewGuid() + Path.GetExtension(request.ImageData.FileName);
                var filePath = Path.Combine("wwwroot/images/Discounts", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageData.CopyToAsync(stream);
                }

                request.ImageUrl = $"/images/Discounts/{fileName}";
            }
            //Mapping between request and book
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
            //
            if (discount.ImageUrl != null)
            {
                var oldFilePath = Path.Combine("wwwroot", discount.ImageUrl.TrimStart('/'));
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
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
