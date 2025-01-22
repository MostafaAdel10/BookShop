using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface ISubSubjectService
    {
        public Task<SubSubject> GetSubSubjectById(int id);
        public Task<List<SubSubject>> GetSubSubjectsListAsync();
    }
}
