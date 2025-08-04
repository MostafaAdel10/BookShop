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
                        IRequestHandler<AddSubjectCommand, Response<SubjectCommand>>,
                        IRequestHandler<EditSubjectCommand, Response<SubjectCommand>>,
                        IRequestHandler<DeleteSubjectCommand, Response<string>>
    {
        #region Fields
        private readonly ISubjectService _subjectService;
        private readonly ISubSubjectService _subSubjectService;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public SubjectCommandHandler(ISubjectService subjectService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer,
            ISubSubjectService subSubjectService, IBookService bookService) : base(stringLocalizer)
        {
            _subjectService = subjectService;
            _mapper = mapper;
            _localizer = stringLocalizer;
            _subSubjectService = subSubjectService;
            _bookService = bookService;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<SubjectCommand>> Handle(AddSubjectCommand request, CancellationToken cancellationToken)
        {
            var subjectEntity = _mapper.Map<DataAccess.Entities.Subject>(request);

            var result = await _subjectService.AddAsync(subjectEntity);

            if (result == "Success")
            {
                var subjectDto = _mapper.Map<SubjectCommand>(subjectEntity);
                return Created(subjectDto);
            }

            return BadRequest<SubjectCommand>();
        }

        public async Task<Response<SubjectCommand>> Handle(EditSubjectCommand request, CancellationToken cancellationToken)
        {
            var subject = await _subjectService.GetByIdAsync(request.Id);
            if (subject is null)
                return NotFound<SubjectCommand>();

            _mapper.Map(request, subject);

            var result = await _subjectService.EditAsync(subject);

            if (result == "Success")
            {
                var subjectDto = _mapper.Map<SubjectCommand>(subject);
                return Success(subjectDto, _localizer[SharedResourcesKeys.Updated]);
            }

            return BadRequest<SubjectCommand>();
        }

        public async Task<Response<string>> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
        {
            var subject = await _subjectService.GetByIdAsync(request.Id);
            if (subject is null)
                return NotFound<string>();

            var isRelatedWithSubSubject = await _subSubjectService.SubjectRelatedWithSubSubject(subject.Id);
            if (isRelatedWithSubSubject)
                return UnprocessableEntity<string>(_localizer[SharedResourcesKeys.ReferencedInAnotherTable]);

            var isRelatedWithBook = await _bookService.SubjectRelatedWithBook(subject.Id);
            if (isRelatedWithBook)
                return UnprocessableEntity<string>(_localizer[SharedResourcesKeys.ReferencedInAnotherTable]);

            var result = await _subjectService.DeleteAsync(subject);

            return result == "Success"
                ? Deleted<string>()
                : BadRequest<string>();
        }

        #endregion
    }
}
