using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface ISubjectService
    {
        public Task<Subject> GetSubjectById(int id);
        public Task<List<Subject>> GetSubjectsListAsync();
        public Task<string> AddAsync(Subject subject);
        public Task<Subject> GetByIdAsync(int id);
        public Task<string> EditAsync(Subject book);
        public Task<string> DeleteAsync(Subject subject);
        public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
        public Task<bool> IsNameExistExcludeSelf(string name, int id);
        public Task<bool> IsNameArExist(string nameAr);
        public Task<bool> IsNameExist(string name);
    }
}
