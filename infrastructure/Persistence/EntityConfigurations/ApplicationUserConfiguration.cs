using Domain.ApplicationUsers;
using Domain.Customers;
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
            value => new CustomerId(value));

        builder.HasOne<Customer>()
            .WithOne()
            .HasForeignKey<ApplicationUser>(au => au.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}