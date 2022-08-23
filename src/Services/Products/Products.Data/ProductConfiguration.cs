using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Domain;

namespace Products.Data
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.OwnsOne(p => p.Picture, a =>
            {
                a.Property(p => p.Url).IsRequired(false);
                a.Property(p => p.DateSet).HasDefaultValue(DateTime.MinValue);
            });

            builder.HasOne(p => p.Category).WithMany(c => c.Products).IsRequired(false);
        }
    }
}