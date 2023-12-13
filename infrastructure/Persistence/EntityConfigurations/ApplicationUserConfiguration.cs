using Domain.ApplicationUsers;
using Domain.Customers;
using Domain.Customers.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Persistence.EntityConfigurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(au => au.CustomerId).HasConversion(
            customerId => customerId.Value, 
            value => CustomerId.Create(value));

        builder.HasOne<Customer>()
            .WithOne()
            .HasForeignKey<ApplicationUser>(au => au.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}