using Core.Data;
using Core.Events;
using Microsoft.EntityFrameworkCore;
using Orders.Domain;

namespace Orders.Data;

public class OrdersDataContext : DataContext
{
    public OrdersDataContext(DbContextOptions options, IDomainEventDispatcher dispatcher) : base(options, dispatcher)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new OrderConfiguration());
    }

    public DbSet<Buyer> Buyers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }

}

