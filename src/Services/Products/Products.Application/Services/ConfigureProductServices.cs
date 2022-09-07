using System;
using Microsoft.Extensions.DependencyInjection;

namespace Products.Application.Services
{
	public static class ConfigureProductServices
	{
        public static IServiceCollection AddProductServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProductsMapper));
            return services;
        }
    }
}

