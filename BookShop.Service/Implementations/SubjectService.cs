using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Abstracts;
using BookShop.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Service.Implementations
{
    public class SubjectService : ISubjectService
    {
        #region Fields
        private readonly ISubjectRepository _subjectRepository;
        #endregion

        #region Constructors
        public SubjectService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }
        #endregion

        #region Handel Functions
        public async Task<Subject> GetSubjectById(int id)
        {
            var subject = await _subjectRepository.GetTableNoTracking().Where(s => s.Id.Equals(id))
                                    .Include(s => s.SubSubjects)
                                    .FirstOrDefaultAsync();
            return subject;
        }
        #endregion


    }
}
