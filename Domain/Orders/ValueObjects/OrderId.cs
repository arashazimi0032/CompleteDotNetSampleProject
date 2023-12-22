using Domain.Primitive.Models;

namespace Domain.Orders.ValueObjects;

public record OrderId : ValueObject
{
    public Guid Value { get; set; }

    private OrderId()
    {

    }

    private OrderId(Guid value)
    {
        Value = value;
    }
    public static OrderId CreateUnique()
    {
        return new OrderId(Guid.NewGuid());
    }

    public static OrderId Create(Guid value)
    {
        return new OrderId(value);
    }
}
