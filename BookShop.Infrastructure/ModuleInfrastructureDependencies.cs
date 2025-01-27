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
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            return services;
        }
    }
}
