using AutoMapper;

namespace BookShop.Core.Mapping.SubSubjects
{
    public partial class SubSubjectProfile : Profile
    {
        public SubSubjectProfile()
        {
            GetSubSubjectByIdMapping();
            GetSubSubjectListMapping();
            AddSubSubjectCommandMapping();
        }
    }
}
