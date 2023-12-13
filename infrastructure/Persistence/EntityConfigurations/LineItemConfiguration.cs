using Domain.Orders.Entities;
using Domain.Orders.ValueObjects;
using Domain.Products;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Persistence.EntityConfigurations;

internal class LineItemConfiguration : IEntityTypeConfiguration<LineItem>
{
    public void Configure(EntityTypeBuilder<LineItem> builder)
    {
        builder.HasKey(li => li.Id);

        builder.Property(li => li.Id).HasConversion(
            lineItemId => lineItemId.Value, 
            value => LineItemId.Create(value));

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(li => li.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(li => li.Price, priceBuilder =>
        {
            priceBuilder.Property(m => m.Amount).HasColumnType("decimal(18, 2)");
            priceBuilder.Property(m => m.Currency).HasMaxLength(3);
        });
    }
}