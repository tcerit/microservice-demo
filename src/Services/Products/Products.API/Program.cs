using System.Reflection;
using Core;
using Core.Data;
using Core.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Products.Application.Commands;
using Products.Application.Services;
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
builder.Services.AddCoreServices();
builder.Services.AddProductServices();
//builder.Services.AddMediatR(typeof(CreateProductCommand).Assembly);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
using (IServiceScope scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    scope.ServiceProvider.GetService<ProductsDataContext>()?.Database.Migrate();
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

