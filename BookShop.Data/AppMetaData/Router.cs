

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
            public const string CreateImages = Prefix + "/CreateImages";
            public const string Edit = Prefix + "/Edit";
            public const string EditUnit_InstockOfBook = Prefix + "/EditUnit_InstockOfBook";
            public const string Delete = Prefix + "/{id}";
            public const string DeleteImageFromBook = Prefix + "/{bookId}/{imageUrl}";
            public const string Paginated = Prefix + "/Paginated";
        }

        public static class SubjectRouting
        {
            public const string Prefix = Rule + "Subject";
            public const string GetById = Prefix + "/Id";
            public const string List = Prefix + "/List";
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + "/{id}";
        }

        public static class SubSubjectRouting
        {
            public const string Prefix = Rule + "SubSubject";
            public const string GetById = Prefix + "/Id";
            public const string List = Prefix + "/List";
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + "/{id}";
        }

        public static class DiscountRouting
        {
            public const string Prefix = Rule + "Discount";
            public const string GetById = Prefix + SingleRoute;
            public const string List = Prefix + "/List";
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + "/{id}";
        }

        public static class Cart_TypeRouting
        {
            public const string Prefix = Rule + "Cart_Type";
            public const string GetById = Prefix + SingleRoute;
            public const string List = Prefix + "/List";
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + "/{id}";
        }

        public static class Shipping_MethodRouting
        {
            public const string Prefix = Rule + "Shipping_Method";
            public const string GetById = Prefix + SingleRoute;
            public const string List = Prefix + "/List";
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + "/{id}";
        }

        public static class ReviewRouting
        {
            public const string Prefix = Rule + "Review";
            public const string GetById = Prefix + SingleRoute;
            public const string List = Prefix + "/List";
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + "/{id}";
        }

        public static class Payment_MethodsRouting
        {
            public const string Prefix = Rule + "Payment_Methods";
            public const string GetById = Prefix + SingleRoute;
            public const string List = Prefix + "/List";
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + "/{id}";
        }

        public static class Order_StateRouting
        {
            public const string Prefix = Rule + "Order_State";
            public const string GetById = Prefix + SingleRoute;
            public const string List = Prefix + "/List";
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + "/{id}";
        }

    }
}
