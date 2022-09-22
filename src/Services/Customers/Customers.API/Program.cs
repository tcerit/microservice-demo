using Core.Configuration;
using Core.Data;
using Customers.Application.Services;
using Customers.Data;
using Microsoft.EntityFrameworkCore;

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
        builder.Services.AddCoreServices();
        builder.Services.AddCustomerServices();

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