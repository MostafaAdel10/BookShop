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
            var shipping_MethodMapper = _mapper.Map<Shipping_Methods>(request);
            //Add
            var result = await _shipping_MethodService.AddAsync(shipping_MethodMapper);

            if (result == "Success")
            {
                // Map back to DTO and return
                var returnShipping_Method = _mapper.Map<Shipping_MethodCommand>(shipping_MethodMapper);
                return Created(returnShipping_Method);
            }
            else
                return BadRequest<Shipping_MethodCommand>();
        }

        public async Task<Response<Shipping_MethodCommand>> Handle(EditShipping_MethodCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var shipping_Method = await _shipping_MethodService.GetShipping_MethodByIdAsync(request.Id);
            //Return NotFound
            if (shipping_Method == null) return NotFound<Shipping_MethodCommand>();
            //Mapping between request and book
            var shipping_MethodMapper = _mapper.Map(request, shipping_Method);
            //Call service that make edit
            var result = await _shipping_MethodService.EditAsync(shipping_MethodMapper);
            //Return response
            if (result == "Success")
            {
                // Map back to DTO and return
                var returnCart_Type = _mapper.Map<Shipping_MethodCommand>(shipping_MethodMapper);
                return Success(returnCart_Type, _localizer[SharedResourcesKeys.Updated]);
            }
            else
                return BadRequest<Shipping_MethodCommand>();
        }

        public async Task<Response<string>> Handle(DeleteShipping_MethodCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var card_Type = await _shipping_MethodService.GetShipping_MethodByIdAsync(request.Id);
            //Return NotFound
            if (card_Type == null) return NotFound<string>();
            //Call service that make delete
            var result = await _shipping_MethodService.DeleteAsync(card_Type);
            //Return response
            if (result == "Success")
                return Deleted<string>();
            else
                return BadRequest<string>();
        }
        #endregion
    }
}
