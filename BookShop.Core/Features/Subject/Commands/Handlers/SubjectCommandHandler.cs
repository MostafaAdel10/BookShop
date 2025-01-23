using AutoMapper;
using BookShop.Core.Bases;
using BookShop.Core.Features.Subject.Commands.Models;
using BookShop.Core.Resources;
using BookShop.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.Subject.Commands.Handlers
{
    public class SubjectCommandHandler : ResponseHandler,
                        IRequestHandler<AddSubjectCommand, Response<string>>,
                        IRequestHandler<EditSubjectCommand, Response<string>>
    {
        #region Fields
        private readonly ISubjectService _subjectService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public SubjectCommandHandler(ISubjectService subjectService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _subjectService = subjectService;
            _mapper = mapper;
            _localizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(AddSubjectCommand request, CancellationToken cancellationToken)
        {
            //Mapping between request and subject
            var subjectMapper = _mapper.Map<DataAccess.Entities.Subject>(request);
            //Add
            var result = await _subjectService.AddAsync(subjectMapper);

            if (result == "Success")
                return Created("");
            else
                return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(EditSubjectCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var subject = await _subjectService.GetByIdAsync(request.Id);
            //Return NotFound
            if (subject == null) return NotFound<string>();
            //Mapping between request and subject
            var subjectMapper = _mapper.Map(request, subject);
            //Call service that make edit
            var result = await _subjectService.EditAsync(subjectMapper);
            //Return response
            if (result == "Success")
                return Success((string)_localizer[SharedResourcesKeys.Updated]);
            else
                return BadRequest<string>();
        }
        #endregion
    }
}
