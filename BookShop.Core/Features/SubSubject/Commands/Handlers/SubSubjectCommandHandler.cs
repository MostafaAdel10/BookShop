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
                        IRequestHandler<AddSubSubjectCommand, Response<SubSubjectCommand>>,
                        IRequestHandler<EditSubSubjectCommand, Response<SubSubjectCommand>>,
                        IRequestHandler<DeleteSubSubjectCommand, Response<string>>
    {
        #region Fields
        private readonly ISubSubjectService _subSubjectService;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public SubSubjectCommandHandler(ISubSubjectService subSubjectService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer, IBookService bookService) : base(stringLocalizer)
        {
            _subSubjectService = subSubjectService;
            _mapper = mapper;
            _localizer = stringLocalizer;
            _bookService = bookService;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<SubSubjectCommand>> Handle(AddSubSubjectCommand request, CancellationToken cancellationToken)
        {
            var subSubjectEntity = _mapper.Map<DataAccess.Entities.SubSubject>(request);

            var result = await _subSubjectService.AddAsync(subSubjectEntity);

            if (result == "Success")
            {
                var subSubjectDto = _mapper.Map<SubSubjectCommand>(subSubjectEntity);
                return Created(subSubjectDto);
            }

            return BadRequest<SubSubjectCommand>();
        }


        public async Task<Response<SubSubjectCommand>> Handle(EditSubSubjectCommand request, CancellationToken cancellationToken)
        {
            var subSubject = await _subSubjectService.GetByIdAsync(request.Id);
            if (subSubject is null)
                return NotFound<SubSubjectCommand>();

            _mapper.Map(request, subSubject);

            var result = await _subSubjectService.EditAsync(subSubject);

            if (result == "Success")
            {
                var dto = _mapper.Map<SubSubjectCommand>(subSubject);
                return Success(dto, _localizer[SharedResourcesKeys.Updated]);
            }

            return BadRequest<SubSubjectCommand>();
        }

        public async Task<Response<string>> Handle(DeleteSubSubjectCommand request, CancellationToken cancellationToken)
        {
            var subSubject = await _subSubjectService.GetByIdAsync(request.Id);
            if (subSubject is null)
                return NotFound<string>();

            var isRelated = await _bookService.SubSubjectRelatedWithBook(subSubject.Id);
            if (isRelated)
                return UnprocessableEntity<string>(_localizer[SharedResourcesKeys.ReferencedInAnotherTable]);

            var result = await _subSubjectService.DeleteAsync(subSubject);

            return result == "Success"
                ? Deleted<string>()
                : BadRequest<string>();
        }
        #endregion
    }
}
