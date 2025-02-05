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
            //Mapping between request and subSubject
            var subSubjectMapper = _mapper.Map<DataAccess.Entities.SubSubject>(request);
            //Add
            var result = await _subSubjectService.AddAsync(subSubjectMapper);

            if (result == "Success")
            {
                // Map back to DTO and return
                var returnSubSubject = _mapper.Map<SubSubjectCommand>(subSubjectMapper);
                return Created(returnSubSubject);
            }
            else
                return BadRequest<SubSubjectCommand>();
        }

        public async Task<Response<SubSubjectCommand>> Handle(EditSubSubjectCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var subSubject = await _subSubjectService.GetByIdAsync(request.Id);
            //Return NotFound
            if (subSubject == null) return NotFound<SubSubjectCommand>();
            //Mapping between request and subSubject
            var subSubjectMapper = _mapper.Map(request, subSubject);
            //Call service that make edit
            var result = await _subSubjectService.EditAsync(subSubjectMapper);
            //Return response
            if (result == "Success")
            {
                // Map back to DTO and return
                var returnSubSubject = _mapper.Map<SubSubjectCommand>(subSubjectMapper);
                return Success(returnSubSubject, _localizer[SharedResourcesKeys.Updated]);
            }
            else
                return BadRequest<SubSubjectCommand>();
        }

        public async Task<Response<string>> Handle(DeleteSubSubjectCommand request, CancellationToken cancellationToken)
        {
            //Check if the id is exist or not
            var subSubject = await _subSubjectService.GetByIdAsync(request.Id);
            //Return NotFound
            if (subSubject == null) return NotFound<string>();
            //Check for linked data
            bool isRelated = await _bookService.SubSubjectRelatedWithBook(subSubject.Id);
            if (isRelated)
            {
                throw new InvalidOperationException("Cannot delete this sub-subject as they have related on other.");
            }
            //Call service that make delete
            var result = await _subSubjectService.DeleteAsync(subSubject);
            //Return response
            if (result == "Success")
                return Deleted<string>();
            else
                return BadRequest<string>();
        }
        #endregion
    }
}
