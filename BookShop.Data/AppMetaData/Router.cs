

namespace BookShop.DataAccess.AppMetaData
{
    public static class Router
    {
        public const string SingleRoute = "/{id}";

        public const string root = "Api";
        public const string version = "V1";
        public const string Rule = root + "/" + version + "/";


        public static class BookRouting
        {
            public const string Prefix = Rule + "Book";
            public const string List = Prefix + "/List";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + "/{id}";
            public const string Paginated = Prefix + "/Paginated";
        }

        public static class SubjectRouting
        {
            public const string Prefix = Rule + "Subject";
            public const string GetById = Prefix + "/Id";
            public const string List = Prefix + "/List";
        }

        public static class SubSubjectRouting
        {
            public const string Prefix = Rule + "SubSubject";
            public const string GetById = Prefix + "/Id";
            public const string List = Prefix + "/List";
        }


    }
}
