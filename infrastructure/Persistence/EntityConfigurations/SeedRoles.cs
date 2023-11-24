using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Persistence.EntityConfigurations;

internal class SeedRoles : IEntityTypeConfiguration<IdentityRole>
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        if (!_roleManager.RoleExistsAsync(Role.Customer.ToString()).GetAwaiter().GetResult())
        {
            builder.HasData(new[]
            {
                new IdentityRole()
                {
                    Name = Role.Admin.ToString(),
                    NormalizedName = Role.Admin.ToString().ToUpper()
                },
                new IdentityRole()
                {
                    Name = Role.Customer.ToString(),
                    NormalizedName = Role.Customer.ToString().ToUpper()
                }
            });
        }
    }
}