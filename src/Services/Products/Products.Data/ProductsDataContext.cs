using System;
using Core.Data;
using Core.Events;
using Microsoft.EntityFrameworkCore;
using Products.Domain;

namespace Products.Data;

public class ProductsDataContext : DataContext
{
    public ProductsDataContext(DbContextOptions options, IDomainEventDispatcher dispatcher) : base(options, dispatcher)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProductConfiguration());
    }

    public DbSet<Product> Products { get; set; }
}

