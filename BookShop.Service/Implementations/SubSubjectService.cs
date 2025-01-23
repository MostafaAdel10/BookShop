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
        private readonly ISubjectRepository _subjectRepository;
        #endregion

        #region Constructors
        public SubSubjectService(ISubSubjectRepository subSubjectRepository, ISubjectRepository subjectRepository)
        {
            _subSubjectRepository = subSubjectRepository;
            _subjectRepository = subjectRepository;
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

        public async Task<bool> IsNameArExistExcludeSelf(string nameAr, int id)
        {
            //Check if the nameAr is Exist Or not
            var subSubject = await _subSubjectRepository.GetTableNoTracking().Where(x => x.Name_Ar.Equals(nameAr) & !x.Id.Equals(id)).FirstOrDefaultAsync();
            if (subSubject == null) return false;
            return true;
        }

        public async Task<bool> IsNameArExist(string nameAr)
        {
            //Check if the nameAr is Exist Or not
            var subSubject = _subSubjectRepository.GetTableNoTracking().Where(x => x.Name_Ar.Equals(nameAr)).FirstOrDefault();
            if (subSubject == null) return false;
            return true;
        }

        public async Task<bool> IsNameExistExcludeSelf(string name, int id)
        {
            //Check if the name is Exist Or not
            var subSubject = await _subSubjectRepository.GetTableNoTracking().Where(x => x.Name.Equals(name) & !x.Id.Equals(id)).FirstOrDefaultAsync();
            if (subSubject == null) return false;
            return true;
        }

        public async Task<bool> IsNameExist(string name)
        {
            //Check if the nameAr is Exist Or not
            var subSubject = _subSubjectRepository.GetTableNoTracking().Where(x => x.Name.Equals(name)).FirstOrDefault();
            if (subSubject == null) return false;
            return true;
        }

        public async Task<bool> IsSubjectIdExist(int subjectId)
        {
            //Check if the nameAr is Exist Or not
            var subject = _subjectRepository.GetTableNoTracking().Where(s => s.Id.Equals(subjectId)).FirstOrDefault();
            if (subject == null) return false;
            return true;
        }

        public async Task<string> AddAsync(SubSubject subSubject)
        {
            //Added SubSubject
            await _subSubjectRepository.AddAsync(subSubject);
            return "Success";
        }

        public async Task<SubSubject> GetByIdAsync(int id)
        {
            var subSubject = await _subSubjectRepository.GetByIdAsync(id);
            return subSubject;
        }

        public async Task<string> EditAsync(SubSubject book)
        {
            await _subSubjectRepository.UpdateAsync(book);
            return "Success";
        }

        #endregion

    }
}
