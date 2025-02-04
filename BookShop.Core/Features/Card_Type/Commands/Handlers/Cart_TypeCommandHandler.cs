using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Card_Type.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Card_Type.Commands.Handlers
{
    public class Cart_TypeCommandHandler : ResponseHandler,
            IRequestHandler<AddCart_TypeCommand, Response<Cart_TypeCommand>>,
            IRequestHandler<EditCart_TypeCommand, Response<Cart_TypeCommand>>,
            IRequestHandler<DeleteCart_TypeCommand, Response<string>>
    {
        #region Fields
        private readonly ICart_TypeService _cart_TypeService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public Cart_TypeCommandHandler(ICart_TypeService cart_TypeService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _cart_TypeService = cart_TypeService;
            _mapper = mapper;
            _localizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<Cart_TypeCommand>> Handle(AddCart_TypeCommand request, CancellationToken cancellationToken)
        {
            //Mapping between request and Cart_Type
            var cart_TypeMapper = _mapper.Map<DataAccess.Entities.Card_Type>(request);
            //Add
            var result = await _cart_TypeService.AddAsync(cart_TypeMapper);

            if (result == "Success")
            {
                // Map back to DTO and return
                var returnCart_Type = _mapper.Map<Cart_TypeCommand>(cart_TypeMapper);
                return Created(returnCart_Type);
            }
            else
                return BadRequest<Cart_TypeCommand>();
        }

        public async Task<Response<Cart_TypeCommand>> Handle(EditCart_TypeCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var card_Type = await _cart_TypeService.GetCart_TypeByIdAsync(request.Id);
            //Return NotFound
            if (card_Type == null) return NotFound<Cart_TypeCommand>();
            //Mapping between request and book
            var cart_TypeMapper = _mapper.Map(request, card_Type);
            //Call service that make edit
            var result = await _cart_TypeService.EditAsync(cart_TypeMapper);
            //Return response
            if (result == "Success")
            {
                // Map back to DTO and return
                var returnCart_Type = _mapper.Map<Cart_TypeCommand>(cart_TypeMapper);
                return Success(returnCart_Type, _localizer[SharedResourcesKeys.Updated]);
            }
            else
                return BadRequest<Cart_TypeCommand>();
        }

        public async Task<Response<string>> Handle(DeleteCart_TypeCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var card_Type = await _cart_TypeService.GetCart_TypeByIdAsync(request.Id);
            //Return NotFound
            if (card_Type == null) return NotFound<string>();
            //Call service that make delete
            var result = await _cart_TypeService.DeleteAsync(card_Type);
            //Return response
            if (result == "Success")
                return Deleted<string>();
            else
                return BadRequest<string>();
        }
        #endregion
    }
}
