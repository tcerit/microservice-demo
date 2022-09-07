using System;
using Core.Events;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Services;

namespace Orders.Application.Services
{
	public static class ConfigureOrdersServices
	{
        public static IServiceCollection AddOrdersServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IOrderRepository), typeof(OrderRepository));
            services.AddAutoMapper(typeof(OrdersMapper));
            return services;
        }
    }
}

