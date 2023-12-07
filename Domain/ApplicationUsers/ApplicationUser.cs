using Domain.Customers;
using Microsoft.AspNetCore.Identity;

namespace Domain.ApplicationUsers;

public class ApplicationUser : IdentityUser
{
    public CustomerId CustomerId { get; set; }
}