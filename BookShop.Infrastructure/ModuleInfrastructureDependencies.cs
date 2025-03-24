using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.InfrastructureBases;
using BookShop.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Infrastructure
{
    public static class ModuleInfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            //configurations
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<ISubjectRepository, SubjectRepository>();
            services.AddTransient<ISubSubjectRepository, SubSubjectRepository>();
            services.AddTransient<IDiscountRepository, DiscountRepository>();
            services.AddTransient<ICart_TypeRepository, Cart_TypeRepository>();
            services.AddTransient<IShipping_MethodRepository, Shipping_MethodRepository>();
            services.AddTransient<IReviewRepository, ReviewRepository>();
            services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddTransient<IBook_DiscountRepository, Book_DiscountRepository>();
            services.AddTransient<IBook_ImageRepository, Book_ImageRepository>();
            services.AddTransient<IPayment_MethodsRepository, Payment_MethodsRepository>();
            services.AddTransient<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddTransient<ICartItemRepository, CartItemRepository>();
            services.AddTransient<IAddressRepository, AddressRepository>();
            services.AddTransient<IOrder_StateRepository, Order_StateRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderItemRepository, OrderItemRepository>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            return services;
        }
    }
}
