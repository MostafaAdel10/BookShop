

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

        public static class OrderRouting
        {
            public const string Prefix = Rule + "Order";
            public const string GetById = Prefix + SingleRoute;
            public const string GetOrdersByUserId = Prefix + "/user/{userId}";
            public const string List = Prefix + "/List";
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string EditOrderState = Prefix + "/orderId/orderStateId";
            public const string CancelOrder = Prefix + "/{id}";
            public const string Paginated = Prefix + "/Paginated";
        }

        public static class OrderItemRouting
        {
            public const string Prefix = Rule + "OrderItem";
            public const string GetById = Prefix + SingleRoute;
            public const string List = Prefix + "/List";
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + "/id/orderId/userId";
        }

        public static class ApplicationUserRouting
        {
            public const string Prefix = Rule + "User";
            public const string Create = Prefix + "/Create";
            public const string Paginated = Prefix + "/Paginated";
            public const string GetByID = Prefix + SingleRoute;
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + "/{id}";
            public const string ChangePassword = Prefix + "/Change-Password";
        }

        public static class Authentication
        {
            public const string Prefix = Rule + "Authentication";
            public const string SignIn = Prefix + "/SignIn";
            public const string RefreshToken = Prefix + "/Refresh-Token";
            public const string ValidateToken = Prefix + "/Validate-Token";
            public const string ConfirmEmail = "/Api/Authentication/ConfirmEmail";
            public const string SendResetPasswordCode = Prefix + "/SendResetPasswordCode";
            public const string ConfirmResetPasswordCode = Prefix + "/ConfirmResetPasswordCode";
            public const string ResetPassword = Prefix + "/ResetPassword";

        }
    }
}
