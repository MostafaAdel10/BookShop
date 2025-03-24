using BookShop.Service.Abstract;
using BookShop.Service.AuthServices.Implementations;
using BookShop.Service.AuthServices.Interfaces;
using BookShop.Service.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Service
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<ISubjectService, SubjectService>();
            services.AddTransient<ISubSubjectService, SubSubjectService>();
            services.AddTransient<IDiscountService, DiscountService>();
            services.AddTransient<ICart_TypeService, Cart_TypeService>();
            services.AddTransient<IShipping_MethodService, Shipping_MethodService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<IBook_ImageService, Book_ImageService>();
            services.AddTransient<IBook_DiscountService, Book_DiscountService>();
            services.AddTransient<IPayment_MethodsService, Payment_MethodsService>();
            services.AddTransient<IShoppingCartService, ShoppingCartService>();
            services.AddTransient<ICartItemService, CartItemService>();
            services.AddTransient<IAddressService, AddressService>();
            services.AddTransient<IOrder_StateService, Order_StateService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IOrderItemService, OrderItemService>();
            services.AddTransient<IApplicationUserService, ApplicationUserService>();
            services.AddTransient<IEmailsService, EmailsService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            return services;
        }
    }
}
