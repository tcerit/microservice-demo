using Core.Data;
using Core.Events;
using Customers.Domain;
using Microsoft.EntityFrameworkCore;

namespace Customers.Data;

public class CustomersDataContext : DataContext
{
    public CustomersDataContext(DbContextOptions options, IDomainEventDispatcher dispatcher) : base(options, dispatcher)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }

    public DbSet<Customer> Customers { get; set; }

  
}

