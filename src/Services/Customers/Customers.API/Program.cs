using System.Reflection;
using Core.Data;
using Core.Events;
using Customers.Application.Commands;
using Customers.Application.Services;
using Customers.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class Program
{
    private static void Main(string[] args)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

        var builder = WebApplication.CreateBuilder(args);

        var dbSettings = builder.Configuration.GetSection("DatabaseSettings");
        builder.Services.AddDbContext<CustomersDataContext>(options =>
        {
            options.EnableSensitiveDataLogging()
                .UseNpgsql(dbSettings.GetValue<string>("ConnectionString")
            );
        });



        builder.Services.AddScoped<DataContext>((serviceProvider) => serviceProvider.GetRequiredService<CustomersDataContext>());
        builder.Services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));

        builder.Services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
        builder.Services.AddMediatR(typeof(DomainEventDispatcher).GetTypeInfo().Assembly);
        builder.Services.AddAutoMapper(typeof(CustomersMapper));
        builder.Services.AddMediatR(typeof(RegisterCustomerCommand).Assembly);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        using (IServiceScope scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            scope.ServiceProvider.GetService<CustomersDataContext>()?.Database.Migrate();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}