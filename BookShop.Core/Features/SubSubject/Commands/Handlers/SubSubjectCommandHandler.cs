using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.SubSubject.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.SubSubject.Commands.Handlers
{
    public class SubSubjectCommandHandler : ResponseHandler,
                        IRequestHandler<AddSubSubjectCommand, Response<string>>,
                        IRequestHandler<EditSubSubjectCommand, Response<string>>
    {
        #region Fields
        private readonly ISubSubjectService _subSubjectService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public SubSubjectCommandHandler(ISubSubjectService subSubjectService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _subSubjectService = subSubjectService;
            _mapper = mapper;
            _localizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(AddSubSubjectCommand request, CancellationToken cancellationToken)
        {
            //Mapping between request and subSubject
            var subSubjectMapper = _mapper.Map<DataAccess.Entities.SubSubject>(request);
            //Add
            var result = await _subSubjectService.AddAsync(subSubjectMapper);

            if (result == "Success")
                return Created("");
            else
                return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(EditSubSubjectCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var subSubject = await _subSubjectService.GetByIdAsync(request.Id);
            //Return NotFound
            if (subSubject == null) return NotFound<string>();
            //Mapping between request and subSubject
            var subSubjectMapper = _mapper.Map(request, subSubject);
            //Call service that make edit
            var result = await _subSubjectService.EditAsync(subSubjectMapper);
            //Return response
            if (result == "Success")
                return Success((string)_localizer[SharedResourcesKeys.Updated]);
            else
                return BadRequest<string>();
        }
        #endregion
    }
}
