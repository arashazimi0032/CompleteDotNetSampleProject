using Domain.Customers.ValueObjects;
using Domain.Primitive.Models;
using Microsoft.AspNetCore.Identity;

namespace Domain.ApplicationUsers;

public class ApplicationUser : IdentityUser, IAuditableEntity
{
    public CustomerId CustomerId { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }
}