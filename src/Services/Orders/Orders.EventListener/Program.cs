using System.Reflection;
using Core;
using Core.Configuration;
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
        //PostgreSQL date issue workaround
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables();
        var configuration = builder.Build();

        var dbSettings = configuration.GetSection("DatabaseSettings");
        services.AddDbContext<OrdersDataContext>(options =>
        {
            options.EnableSensitiveDataLogging()
                .UseNpgsql(dbSettings.GetValue<string>("ConnectionString"));
        });
        services.AddScoped<DataContext>((serviceProvider) => serviceProvider.GetRequiredService<OrdersDataContext>());
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies(), c => c.AsSingleton());
        services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddSingleton(typeof(IRepositoryScopeFactory<>), typeof(RepositoryScopeFactory<>));

        services.Configure<MessageBrokerSettings>(configuration.GetSection("MessageBroker"));

        services.AddHostedService<OrdersEventListenerWorker>();
    })
    .ConfigureLogging((context, logging) =>
    {
        var env = context.HostingEnvironment;
        var config = context.Configuration.GetSection("Logging");
        logging.AddConfiguration(config);
        logging.AddConsole();
        logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
    })
    .Build();

await host.RunAsync();
