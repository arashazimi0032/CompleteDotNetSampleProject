using Domain.Products;

namespace Domain.Orders;

public record LineItemId
{
    public Guid Value { get; set; }
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
