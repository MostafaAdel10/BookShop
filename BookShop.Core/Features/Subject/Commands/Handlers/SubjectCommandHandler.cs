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
                        IRequestHandler<EditSubjectCommand, Response<string>>,
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

        public async Task<Response<string>> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var subject = await _subjectService.GetByIdAsync(request.Id);
            //Return NotFound
            if (subject == null) return NotFound<string>();
            //Check for linked data
            bool isRelatedWithSubSubject = await _subSubjectService.SubjectRelatedWithBook(subject.Id);
            if (isRelatedWithSubSubject)
            {
                throw new InvalidOperationException("Cannot delete this subject as they have related on other.");
            }

            bool isRelatedWithBook = await _bookService.SubjectRelatedWithBook(subject.Id);
            if (isRelatedWithBook)
            {
                throw new InvalidOperationException("Cannot delete this subject as they have related on other.");
            }
            //Call service that make delete
            var result = await _subjectService.DeleteAsync(subject);
            //Return response
            if (result == "Success")
                return Deleted<string>();
            else
                return BadRequest<string>();
        }
        #endregion
    }
}
