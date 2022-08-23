﻿using System.Reflection;
using Core.Data;
using Core.Events;
using Core.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Orders.Data;
using Orders.MessageRelay;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables();
        var configuration = builder.Build();
        var dbSettings = configuration.GetSection("DatabaseSettings");
        services.AddDbContext<OrdersDataContext>(options => {
            options.EnableSensitiveDataLogging()
                .UseNpgsql(dbSettings.GetValue<string>("ConnectionString")
                );
        }, ServiceLifetime.Singleton);
        services.AddSingleton<DataContext>((serviceProvider) => serviceProvider.GetRequiredService<OrdersDataContext>());
        services.Configure<MessageBrokerSettings>(configuration.GetSection("MessageBroker"));
        services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddMediatR(typeof(DomainEventDispatcher).GetTypeInfo().Assembly);

        services.AddHostedService<OrdersMessageRelayWorker>();
    })
    .Build();

await host.RunAsync();

