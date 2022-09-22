using Core.Configuration;
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

        services.Configure<MessageBrokerSettings>(configuration.GetSection("MessageBroker"));

        services.AddDbContext<CustomersDataContext>(options =>
        {
            options.EnableSensitiveDataLogging()
                .UseNpgsql(configuration.GetSection("DatabaseSettings").GetValue<string>("ConnectionString"));
        });
        services.AddScoped<DataContext>((serviceProvider) => serviceProvider.GetRequiredService<CustomersDataContext>());
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies(), c => c.AsSingleton());
        services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddSingleton(typeof(IRepositoryScopeFactory<>), typeof(RepositoryScopeFactory<>));

        services.AddHostedService<CustomerEventListenerWorker>();
    })
    .Build();

await host.RunAsync();

