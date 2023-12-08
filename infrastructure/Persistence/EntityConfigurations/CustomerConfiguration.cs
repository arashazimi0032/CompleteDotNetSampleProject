using Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Persistence.EntityConfigurations;

internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).HasConversion(
            customerId => customerId.Value,
            value => CustomerId.Create(value));

        builder.Property(c => c.Email).HasMaxLength(255).IsRequired();
        
        builder.Property(c => c.Name).HasMaxLength(150);

        builder.HasIndex(c => c.Email).IsUnique();
    }
}