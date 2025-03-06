using AutoMapper;

namespace BookShop.Core.Mapping.SubSubjects
{
    public partial class SubSubjectProfile : Profile
    {
        public SubSubjectProfile()
        {
            GetBooksBySubSubjectIdMapping();
            GetSubSubjectByIdMapping();
            GetSubSubjectListMapping();
            AddSubSubjectCommandMapping();
            EditSubSubjectCommandMapping();
            SubSubjectCommandMapping();
        }
    }
}
