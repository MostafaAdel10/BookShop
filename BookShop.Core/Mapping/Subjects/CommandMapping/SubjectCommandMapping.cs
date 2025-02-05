using BookShop.Core.Features.Subject.Commands.Models;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.Subjects
{
    public partial class SubjectProfile
    {
        public void SubjectCommandMapping()
        {
            CreateMap<Subject, SubjectCommand>();
        }
    }
}
