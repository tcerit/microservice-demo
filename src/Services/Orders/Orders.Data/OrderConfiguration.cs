using System;
using Core.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain;


namespace Orders.Data
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsMany(p => p.Items).HasOne(p => p.Product).WithMany();

            builder.HasOne(p => p.Buyer).WithMany(p => p.Orders);

            builder.Property(p => p.Status).HasConversion<int>(new EnumerationConverter<OrderStatus>());

        }
    }
}
