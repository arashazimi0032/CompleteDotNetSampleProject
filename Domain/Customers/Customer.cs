using Domain.Primitive;

namespace Domain.Customers;

public class Customer : Entity
{
    private Customer()
    {
    }
    public static Customer Create(string name, string email)
    {
        return new Customer
        {
            Id = new CustomerId(Guid.NewGuid()),
            Name = name,
            Email = email,
        };
    }

    public void Update(string? name, string? email)
    {
        if (name is not null) Name = name;
        if (email is not null) Email = email;
    }

    public CustomerId Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
}
