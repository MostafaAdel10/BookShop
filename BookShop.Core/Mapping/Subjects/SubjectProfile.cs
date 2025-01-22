using AutoMapper;

namespace BookShop.Core.Mapping.Subjects
{
    public partial class SubjectProfile : Profile
    {
        public SubjectProfile()
        {
            GetSubjectByIdMapping();
            GetSubjectListMapping();
        }
    }
}
