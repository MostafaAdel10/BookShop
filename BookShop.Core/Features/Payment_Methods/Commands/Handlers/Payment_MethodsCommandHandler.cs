using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Payment_Methods.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Payment_Methods.Commands.Handlers
{
    public class Payment_MethodsCommandHandler : ResponseHandler,
            IRequestHandler<AddPayment_MethodsCommand, Response<Payment_MethodsCommand>>,
            IRequestHandler<EditPayment_MethodsCommand, Response<Payment_MethodsCommand>>,
            IRequestHandler<DeletePayment_MethodsCommand, Response<string>>
    {
        #region Fields
        private readonly IPayment_MethodsService _payment_MethodsService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public Payment_MethodsCommandHandler(IPayment_MethodsService payment_MethodsService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _payment_MethodsService = payment_MethodsService;
            _mapper = mapper;
            _localizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<Payment_MethodsCommand>> Handle(AddPayment_MethodsCommand request, CancellationToken cancellationToken)
        {
            //Mapping between request and payment_Method
            var payment_MethodMapper = _mapper.Map<DataAccess.Entities.Payment_Methods>(request);
            //Add
            var result = await _payment_MethodsService.AddAsync(payment_MethodMapper);

            if (result == "Success")
            {
                // Map back to DTO and return
                var returnPayment_Method = _mapper.Map<Payment_MethodsCommand>(payment_MethodMapper);
                return Created(returnPayment_Method);
            }
            else
                return BadRequest<Payment_MethodsCommand>();
        }

        public async Task<Response<Payment_MethodsCommand>> Handle(EditPayment_MethodsCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var payment_Method = await _payment_MethodsService.GetPayment_MethodByIdAsync(request.Id);
            //Return NotFound
            if (payment_Method == null) return NotFound<Payment_MethodsCommand>();
            //Mapping between request and payment_Method
            var payment_MethodMapper = _mapper.Map(request, payment_Method);
            //Call service that make edit
            var result = await _payment_MethodsService.EditAsync(payment_MethodMapper);
            //Return response
            if (result == "Success")
            {
                // Map back to DTO and return
                var returnPayment_Method = _mapper.Map<Payment_MethodsCommand>(payment_MethodMapper);
                return Success(returnPayment_Method, _localizer[SharedResourcesKeys.Updated]);
            }
            else
                return BadRequest<Payment_MethodsCommand>();
        }

        public async Task<Response<string>> Handle(DeletePayment_MethodsCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var payment_Method = await _payment_MethodsService.GetPayment_MethodByIdAsync(request.Id);
            //Return NotFound
            if (payment_Method == null) return NotFound<string>();
            //Call service that make delete
            var result = await _payment_MethodsService.DeleteAsync(payment_Method);
            //Return response
            if (result == "Success")
                return Deleted<string>();
            else
                return BadRequest<string>();
        }
        #endregion
    }
}
