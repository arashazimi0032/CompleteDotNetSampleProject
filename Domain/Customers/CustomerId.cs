namespace Domain.Customers;

public record CustomerId
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
