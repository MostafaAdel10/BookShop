using BookShop.Infrastructure.Abstracts;
using BookShop.Infrastructure.Repository;
using BookShop.Service.Abstract;
using BookShop.Service.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Service
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.AddTransient<IBookService, BookService>();
            return services;
        }
    }
}
