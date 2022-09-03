using System.Reflection;
using Core.Data;
using Core.Events;
using Core.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Orders.Data;
using Orders.EventListener;
using Orders.EventListener.Events;

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
                .UseNpgsql(dbSettings.GetValue<string>("ConnectionString"));
        }, ServiceLifetime.Singleton);
        services.AddSingleton<DataContext>((serviceProvider) => serviceProvider.GetRequiredService<OrdersDataContext>());
        services.Configure<MessageBrokerSettings>(configuration.GetSection("MessageBroker"));
        services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddMediatR(typeof(DomainEventDispatcher).GetTypeInfo().Assembly);
        services.AddMediatR(typeof(CustomerCreatedEvent).Assembly);
        services.AddHostedService<OrdersEventListenerWorker>();
    })
    .ConfigureLogging((context, logging) => {
        var env = context.HostingEnvironment;
        var config = context.Configuration.GetSection("Logging");
        logging.AddConfiguration(config);
        logging.AddConsole();
        logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
    })
    .Build();

await host.RunAsync();
