

namespace BookShop.DataAccess.AppMetaData
{
    public static class Router
    {
        public const string SingleRoute = "/{id}";

        public const string root = "Api";
        public const string version = "V1";
        public const string Rule = root + "/" + version + "/";

        public static class PaymentRouting
        {
            public const string Prefix = Rule + "Payment";
            public const string VodafoneCashTransaction = Prefix + "/VodafoneCashTransaction";
            public const string EtisalatCashTransaction = Prefix + "/EtisalatCashTransaction";
        }
        public static class BookRouting
        {
            public const string Prefix = Rule + "Book";
            public const string List = Prefix + "/List";
            public const string GetById = Prefix + SingleRoute;
            public const string Create = Prefix + "/Create";
            public const string CreateImages = Prefix + "/CreateImages";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + "/{id}";
            public const string DeleteImageFromBook = Prefix + "/DeleteImageFromBook";
            public const string DeleteDiscountFromBooks = Prefix + "/DeleteDiscountFromBooks/{discountId}";
            public const string Paginated = Prefix + "/Paginated";
        }

        public static class SubjectRouting
        {
            public const string Prefix = Rule + "Subject";
            public const string GetById = Prefix + SingleRoute;
            public const string GetBooksBySubjectId = Prefix + "/BooksBy/subjectId";
            public const string List = Prefix + "/List";
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + "/{id}";
        }

        public static class SubSubjectRouting
        {
            public const string Prefix = Rule + "SubSubject";
            public const string GetById = Prefix + SingleRoute;
            public const string GetBooksBySubSubjectId = Prefix + "/BooksBy/subSubjectId";
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
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + "/{id}";
            public const string GetReviewsByBookId = Prefix + "/{bookId}";
            public const string Paginated = Prefix + "/Paginated";
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
            public const string GetOrdersByCurrentUser = Prefix + "/GetOrdersByCurrentUser";
            public const string List = Prefix + "/List";
            public const string Paginated = Prefix + "/Paginated";
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string EditOrderState = Prefix + "/orderId/orderStateId";
            public const string CancelOrder = Prefix + "/CancelOrder/{id}";
        }

        public static class CartItemRouting
        {
            public const string Prefix = Rule + "CartItem";
            public const string GetById = Prefix + SingleRoute;
            public const string List = Prefix + "/List";
            public const string GetCurrentUser_sCartItems = Prefix + "/GetCurrentUser_sCartItems";
            public const string Create = Prefix + "/Create";
            public const string EditTheCartItemQuantityAndCheckIfItIsInStockCommand = Prefix + "/EditTheCartItemQuantityAndCheckIfItIsInStockCommand";
            public const string Delete = Prefix + "/{id}";
        }

        public static class ShippingAddressRouting
        {
            public const string Prefix = Rule + "ShippingAddress";
            public const string GetCurrentUser_sShippingAddresses = Prefix + "/GetCurrentUser_sShippingAddresses";
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

        public static class AuthorizationRouting
        {
            public const string Prefix = Rule + "AuthorizationRouting";
            public const string Roles = Prefix + "/Roles";
            public const string Claims = Prefix + "/Claims";
            public const string Create = Roles + "/Create";
            public const string Edit = Roles + "/Edit";
            public const string Delete = Roles + "/Delete/{id}";
            public const string RoleList = Roles + "/Role-List";
            public const string GetRoleById = Roles + "/Role-By-Id/{id}";
            public const string ManageUserRoles = Roles + "/Manage-User-Roles/{userId}";
            public const string ManageUserClaims = Claims + "/Manage-User-Claims/{userId}";
            public const string UpdateUserRoles = Roles + "/Update-User-Roles";
            public const string UpdateUserClaims = Claims + "/Update-User-Claims";
        }

        public static class EmailsRoute
        {
            public const string Prefix = Rule + "EmailsRoute";
            public const string SendEmail = Prefix + "/SendEmail";
        }
    }
}
