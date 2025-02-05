using BookShop.Core.Features.SubSubject.Commands.Models;
using BookShop.DataAccess.Entities;

namespace BookShop.Core.Mapping.SubSubjects
{
    public partial class SubSubjectProfile
    {
        public void SubSubjectCommandMapping()
        {
            CreateMap<SubSubject, SubSubjectCommand>();
        }
    }
}
