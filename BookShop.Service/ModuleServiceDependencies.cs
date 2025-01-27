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
            services.AddTransient<ISubjectService, SubjectService>();
            services.AddTransient<ISubSubjectService, SubSubjectService>();
            services.AddTransient<IDiscountService, DiscountService>();
            return services;
        }
    }
}
