using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Shipping_Method.Commands.Models;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Shipping_Method.Commands.Handlers
{
    public class Shipping_MethodCommandHandler : ResponseHandler,
            IRequestHandler<AddShipping_MethodCommand, Response<Shipping_MethodCommand>>,
            IRequestHandler<EditShipping_MethodCommand, Response<Shipping_MethodCommand>>,
            IRequestHandler<DeleteShipping_MethodCommand, Response<string>>
    {
        #region Fields
        private readonly IShipping_MethodService _shipping_MethodService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public Shipping_MethodCommandHandler(IShipping_MethodService shipping_MethodService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _shipping_MethodService = shipping_MethodService;
            _mapper = mapper;
            _localizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<Shipping_MethodCommand>> Handle(AddShipping_MethodCommand request, CancellationToken cancellationToken)
        {
            //Mapping between request and Shipping_Method
            var shippingMethodEntity = _mapper.Map<Shipping_Methods>(request);

            //Add
            var result = await _shipping_MethodService.AddAsync(shippingMethodEntity);

            //Check if the result is success or not
            if (result == "Success")
            {
                var returnDto = _mapper.Map<Shipping_MethodCommand>(shippingMethodEntity);
                return Created(returnDto);
            }

            return BadRequest<Shipping_MethodCommand>(_localizer[SharedResourcesKeys.AddFailed]);
        }

        public async Task<Response<Shipping_MethodCommand>> Handle(EditShipping_MethodCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var shippingMethod = await _shipping_MethodService.GetShipping_MethodByIdAsync(request.Id);
            if (shippingMethod == null)
                return NotFound<Shipping_MethodCommand>();

            // Map the request to the existing entity
            var updatedEntity = _mapper.Map(request, shippingMethod);

            // Map the updated entity back to the DTO
            var result = await _shipping_MethodService.EditAsync(updatedEntity);

            // Check if the result is success or not
            if (result == "Success")
            {
                var returnDto = _mapper.Map<Shipping_MethodCommand>(updatedEntity);
                return Success(returnDto, _localizer[SharedResourcesKeys.Updated]);
            }
            return BadRequest<Shipping_MethodCommand>(_localizer[SharedResourcesKeys.UpdateFailed]);
        }

        public async Task<Response<string>> Handle(DeleteShipping_MethodCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var shippingMethod = await _shipping_MethodService.GetShipping_MethodByIdAsync(request.Id);
            if (shippingMethod == null)
                return NotFound<string>();

            //Delete
            var result = await _shipping_MethodService.DeleteAsync(shippingMethod);

            if (result == "Success")
                return Deleted<string>();

            return BadRequest<string>(_localizer[SharedResourcesKeys.DeletedFailed]);
        }
        #endregion
    }
}
