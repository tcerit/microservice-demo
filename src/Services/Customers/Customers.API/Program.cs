using System.Reflection;
using Core.Data;
using Core.Events;
using Customers.Application.Commands;
using Customers.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

var builder = WebApplication.CreateBuilder(args);

var dbSettings = builder.Configuration.GetSection("DatabaseSettings");
builder.Services.AddDbContext<CustomersDataContext>(options => {
    options.EnableSensitiveDataLogging()
        .UseNpgsql(dbSettings.GetValue<string>("ConnectionString")
    );
});



builder.Services.AddScoped<DataContext>((serviceProvider) => serviceProvider.GetRequiredService<CustomersDataContext>());
builder.Services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));

builder.Services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
builder.Services.AddMediatR(typeof(DomainEventDispatcher).GetTypeInfo().Assembly);

//builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMediatR(typeof(RegisterCustomerCommand).Assembly);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
