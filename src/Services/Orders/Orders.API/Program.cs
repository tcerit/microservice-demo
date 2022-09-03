using System.Reflection;
using Core.Data;
using Core.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Orders.Application;
using Orders.Application.Repositories;
using Orders.Application.Services;
using Orders.Data;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

var builder = WebApplication.CreateBuilder(args);

var dbSettings = builder.Configuration.GetSection("DatabaseSettings");
var xx = dbSettings.GetValue<string>("ConnectionString");
// Add services to the container.
builder.Services.AddDbContext<OrdersDataContext>(options => {
    options.EnableSensitiveDataLogging()
        .UseNpgsql(dbSettings.GetValue<string>("ConnectionString"));
});

builder.Services.AddScoped<DataContext>((serviceProvider) => serviceProvider.GetRequiredService<OrdersDataContext>());
builder.Services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddTransient(typeof(IOrderRepository), typeof(OrderRepository));
builder.Services.AddAutoMapper(typeof(OrdersMapper));
builder.Services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
builder.Services.AddMediatR(typeof(DomainEventDispatcher).GetTypeInfo().Assembly);
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
Assembly[] y = AppDomain.CurrentDomain.GetAssemblies();
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

using (IServiceScope scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    scope.ServiceProvider.GetService<OrdersDataContext>()?.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

