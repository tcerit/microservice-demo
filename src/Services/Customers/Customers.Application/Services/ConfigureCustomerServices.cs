using System;
using Microsoft.Extensions.DependencyInjection;

namespace Customers.Application.Services
{
	public static class ConfigureCustomerServices
	{
        public static IServiceCollection AddCustomerServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(CustomersMapper));
            return services;
        }
    }
}

