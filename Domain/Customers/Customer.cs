using Domain.Primitive;

namespace Domain.Customers;

public class Customer : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
