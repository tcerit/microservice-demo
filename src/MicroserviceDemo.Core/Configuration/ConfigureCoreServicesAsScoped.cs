using System;
using Core.Data;
using Core.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;

namespace Core.Configuration
{
    public static class ConfigureCoreServicesAsScoped
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddMediatR(typeof(DomainEventDispatcher).GetTypeInfo().Assembly);
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}

