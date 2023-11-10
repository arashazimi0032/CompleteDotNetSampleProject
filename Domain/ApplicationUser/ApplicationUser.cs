using Microsoft.AspNetCore.Identity;

namespace Domain.ApplicationUser;

public class ApplicationUser : IdentityUser
{
    public Guid? CustomerId { get; set; }
}