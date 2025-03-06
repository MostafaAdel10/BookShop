using AutoMapper;

namespace BookShop.Core.Mapping.Subjects
{
    public partial class SubjectProfile : Profile
    {
        public SubjectProfile()
        {
            GetBooksBySubjectIdMapping();
            GetSubjectByIdMapping();
            GetSubjectListMapping();
            AddSubjectCommandMapping();
            EditSubjectCommandMapping();
            SubjectCommandMapping();
        }
    }
}
