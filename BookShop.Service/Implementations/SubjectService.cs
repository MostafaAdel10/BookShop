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

        public async Task<string> AddAsync(Subject subject)
        {
            //Added Subject
            await _subjectRepository.AddAsync(subject);
            return "Success";
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

        public async Task<List<Subject>> GetSubjectsListAsync()
        {
            return await _subjectRepository.GetSubjectsListAsync();
        }

        public async Task<bool> IsNameArExistExcludeSelf(string nameAr, int id)
        {
            //Check if the nameAr is Exist Or not
            var subject = await _subjectRepository.GetTableNoTracking().Where(x => x.Name_Ar.Equals(nameAr) & !x.Id.Equals(id)).FirstOrDefaultAsync();
            if (subject == null) return false;
            return true;
        }

        public async Task<bool> IsNameArExist(string nameAr)
        {
            //Check if the nameAr is Exist Or not
            var subject = _subjectRepository.GetTableNoTracking().Where(x => x.Name_Ar.Equals(nameAr)).FirstOrDefault();
            if (subject == null) return false;
            return true;
        }

        public async Task<bool> IsNameExistExcludeSelf(string name, int id)
        {
            //Check if the name is Exist Or not
            var subject = await _subjectRepository.GetTableNoTracking().Where(x => x.Name.Equals(name) & !x.Id.Equals(id)).FirstOrDefaultAsync();
            if (subject == null) return false;
            return true;
        }

        public async Task<bool> IsNameExist(string name)
        {
            //Check if the nameAr is Exist Or not
            var subject = _subjectRepository.GetTableNoTracking().Where(x => x.Name.Equals(name)).FirstOrDefault();
            if (subject == null) return false;
            return true;
        }
        #endregion


    }
}
