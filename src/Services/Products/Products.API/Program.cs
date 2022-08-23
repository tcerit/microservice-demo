using System.Reflection;
using Core.Data;
using Core.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Products.Application.Commands;
using Products.Data;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

var dbSettings = builder.Configuration.GetSection("DatabaseSettings");
builder.Services.AddDbContext<ProductsDataContext>(options => {
    options.EnableSensitiveDataLogging()
        .UseNpgsql(dbSettings.GetValue<string>("ConnectionString")
    );
});

builder.Services.AddScoped<DataContext>((serviceProvider) => serviceProvider.GetRequiredService<ProductsDataContext>());
builder.Services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));

builder.Services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
builder.Services.AddMediatR(typeof(DomainEventDispatcher).GetTypeInfo().Assembly);

builder.Services.AddMediatR(typeof(CreateProductCommand).Assembly);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

