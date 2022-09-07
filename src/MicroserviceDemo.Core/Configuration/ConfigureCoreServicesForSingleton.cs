using System;
using Core.Data;
using Core.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Configuration
{
	public static class ConfigureCoreServicesForSingleton
	{
        public static IServiceCollection AddCoreServicesForSingleton(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies(), c => c.AsSingleton());
            services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddSingleton(typeof(IRepositoryScopeFactory<>), typeof(RepositoryScopeFactory<>));
            return services;
        }
	}
}

