using Domain.Primitive.Models;

namespace Domain.Customers.ValueObjects;

public record CustomerId : ValueObject
{
    public Guid Value { get; set; }
    private CustomerId(Guid value)
    {
        Value = value;
    }
    public static CustomerId CreateUnique()
    {
        return new CustomerId(Guid.NewGuid());
    }

    public static CustomerId Create(Guid value)
    {
        return new CustomerId(value);
    }
}
