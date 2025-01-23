using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface ISubSubjectService
    {
        public Task<SubSubject> GetSubSubjectById(int id);
        public Task<List<SubSubject>> GetSubSubjectsListAsync();
        public Task<string> AddAsync(SubSubject subSubject);
        public Task<SubSubject> GetByIdAsync(int id);
        public Task<string> EditAsync(SubSubject book);
        public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
        public Task<bool> IsNameExistExcludeSelf(string name, int id);
        public Task<bool> IsNameArExist(string nameAr);
        public Task<bool> IsNameExist(string name);
        public Task<bool> IsSubjectIdExist(int subjectId);
    }
}
