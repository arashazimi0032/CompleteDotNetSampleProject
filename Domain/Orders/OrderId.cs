namespace Domain.Orders;

public record OrderId
{
    public Guid Value { get; set; }
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
