using Microsoft.AspNetCore.Identity;

namespace Domain.ApplicationUsers;

public class ApplicationUser : IdentityUser
{
    public Guid? CustomerId { get; set; }
}