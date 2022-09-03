using System.Reflection;
using Core.Data;
using Core.Events;
using Core.Settings;
using Customers.Data;
using Customers.EventListener;
using MediatR;
using Microsoft.EntityFrameworkCore;


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var builder = new ConfigurationBuilder().AddEnvironmentVariables();
        var configuration = builder.Build();
        var dbSettings = configuration.GetSection("DatabaseSettings");
        services.AddDbContext<CustomersDataContext>(options => {
            options.UseNpgsql(dbSettings.GetValue<string>("ConnectionString"));
        }, ServiceLifetime.Singleton);
        services.AddSingleton<DataContext>((serviceProvider) => serviceProvider.GetRequiredService<CustomersDataContext>());
        services.Configure<MessageBrokerSettings>(configuration.GetSection("MessageBroker"));
        services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddMediatR(typeof(DomainEventDispatcher).GetTypeInfo().Assembly);
        services.AddHostedService<CustomerEventListenerWorker>();
    })
    .Build();

await host.RunAsync();

