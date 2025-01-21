using BookShop.DataAccess.Entities;

namespace BookShop.Service.Abstract
{
    public interface ISubjectService
    {
        public Task<Subject> GetSubjectById(int id);
    }
}
