using System.Reflection;
using Core.Data;
using Core.Events;
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
        }, ServiceLifetime.Singleton);
        services.AddSingleton<DataContext>((serviceProvider) => serviceProvider.GetRequiredService<ProductsDataContext>());
        services.Configure<MessageBrokerSettings>(configuration.GetSection("MessageBroker"));
        services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddMediatR(typeof(DomainEventDispatcher).GetTypeInfo().Assembly);
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

