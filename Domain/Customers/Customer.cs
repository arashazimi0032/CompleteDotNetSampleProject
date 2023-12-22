using Domain.Customers.ValueObjects;
using Domain.Primitive.Models;

namespace Domain.Customers;

public class Customer : Entity<CustomerId>
{
    private Customer()
    {
        
    }
    private Customer(CustomerId id, string name, string email) : base(id)
    {
        Name = name;
        Email = email;
    }
    public static Customer Create(string name, string email)
    {
        return new Customer(CustomerId.CreateUnique(), name, email);
    }

    public void Update(string? name, string? email)
    {
        if (name is not null) Name = name;
        if (email is not null) Email = email;
    }

    public string Name { get; private set; }
    public string Email { get; private set; }
}
