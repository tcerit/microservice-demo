using Core.Configuration;
using Core.Data;
using Core.Data.Outbox;
using Core.Events;
using Core.Messaging;
using Core.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.Data;
using Products.MessageRelay;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables();
        var configuration = builder.Build();

        var dbSettings = configuration.GetSection("DatabaseSettings");
        services.AddDbContext<ProductsDataContext>(options => {
            options.UseNpgsql(dbSettings.GetValue<string>("ConnectionString"));
        });

        services.AddScoped<DataContext>((serviceProvider) => serviceProvider.GetRequiredService<ProductsDataContext>());

        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies(), c => c.AsSingleton());
        services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddSingleton(typeof(IRepositoryScopeFactory<>), typeof(RepositoryScopeFactory<>));
        services.Configure<MessageBrokerSettings>(configuration.GetSection("MessageBroker"));
        services.AddSingleton<IMessageBroker, MessageBroker>();
        services.AddSingleton<IOutboxManager, OutboxManager<ProductsDataContext>>();
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

        services.AddHostedService<ProductMessageRelayWorker>();
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

