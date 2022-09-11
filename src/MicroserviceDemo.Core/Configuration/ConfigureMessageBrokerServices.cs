using System;
using Core.Data.Outbox;
using Core.Messaging;
using Core.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Configuration
{
    public static class ConfigureMessageBrokerServices
    {
        public static IServiceCollection AddMessageBrokerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MessageBrokerSettings>(configuration.GetSection("MessageBrokerSettings"));
            services.AddSingleton<IMessageBroker, MessageBroker>();
            services.AddSingleton(typeof(IOutboxManager),typeof(OutboxManager<>));
            return services;
        }
    }
}

