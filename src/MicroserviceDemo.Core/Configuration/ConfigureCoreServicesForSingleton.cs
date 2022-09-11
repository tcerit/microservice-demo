using System;
using Core.Data;
using Core.Events;
using Core.Settings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Configuration
{
	public static class ConfigureCoreServicesForSingleton
	{
        public static IServiceCollection AddCoreServicesForSingleton(this IServiceCollection services, DataContext context)
        {

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies(), c => c.AsSingleton());
            services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddSingleton(typeof(IRepositoryScopeFactory<>), typeof(RepositoryScopeFactory<>));
            return services;
        }
	}
}

