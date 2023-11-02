using Domain.Orders;
using Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Persistence.EntityConfigurations;

internal class LineItemConfiguration : IEntityTypeConfiguration<LineItem>
{
    public void Configure(EntityTypeBuilder<LineItem> builder)
    {
        builder.HasKey(li => li.Id);

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(li => li.ProductId);
    }
}