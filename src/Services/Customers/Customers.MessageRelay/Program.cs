using Core.Configuration;
using Core.Data;
using Core.Data.Outbox;
using Core.Events;
using Core.Messaging;
using Core.Settings;
using Customers.Data;
using Customers.MessageRelay;
using MediatR;
using Microsoft.EntityFrameworkCore;


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables();
        var configuration = builder.Build();

        services.Configure<MessageBrokerSettings>(configuration.GetSection("MessageBroker"));

        services.AddDbContext<CustomersDataContext>(options => {
            options.UseNpgsql(configuration.GetSection("DatabaseSettings").GetValue<string>("ConnectionString"));
        });

        services.AddScoped<DataContext>((serviceProvider) => serviceProvider.GetRequiredService<CustomersDataContext>());
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies(), c => c.AsSingleton());
        services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddSingleton(typeof(IRepositoryScopeFactory<>), typeof(RepositoryScopeFactory<>));
        services.AddSingleton<IMessageBroker, MessageBroker>();
        services.AddSingleton<IOutboxManager, OutboxManager<CustomersDataContext>>();

        services.AddHostedService<CustomersMessageRelayWorker>();
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
