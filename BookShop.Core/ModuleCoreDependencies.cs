using BookShop.Service.Abstract;
using BookShop.Service.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BookShop.Core
{
    public static class ModuleCoreDependencies
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            //Register MediatR On Assembly => dll
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            return services;
        }
    }
}
