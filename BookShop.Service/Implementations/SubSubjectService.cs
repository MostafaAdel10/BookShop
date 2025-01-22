using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Service.Implementations
{
    public class SubSubjectService : ISubSubjectService
    {
        #region Fields
        private readonly ISubSubjectRepository _subSubjectRepository;
        #endregion

        #region Constructors
        public SubSubjectService(ISubSubjectRepository subSubjectRepository)
        {
            _subSubjectRepository = subSubjectRepository;
        }
        #endregion

        #region Handel Functions
        public async Task<SubSubject> GetSubSubjectById(int id)
        {
            var subSubject = await _subSubjectRepository.GetTableNoTracking().Where(ss => ss.Id.Equals(id))
                                    .Include(ss => ss.Subject)
                                    .FirstOrDefaultAsync();
            return subSubject;
        }

        public async Task<List<SubSubject>> GetSubSubjectsListAsync()
        {
            return await _subSubjectRepository.GetSubSubjectsListAsync();
        }
        #endregion

    }
}
