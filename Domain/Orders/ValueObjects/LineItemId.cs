using Domain.Primitive.Models;

namespace Domain.Orders.ValueObjects;

public record LineItemId : ValueObject
{
    public Guid Value { get; set; }

    private LineItemId()
    {

    }

    private LineItemId(Guid value)
    {
        Value = value;
    }
    public static LineItemId CreateUnique()
    {
        return new LineItemId(Guid.NewGuid());
    }

    public static LineItemId Create(Guid value)
    {
        return new LineItemId(value);
    }
}
