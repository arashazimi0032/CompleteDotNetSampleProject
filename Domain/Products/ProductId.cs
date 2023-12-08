using Domain.Customers;

namespace Domain.Products;

public record ProductId
{
    public Guid Value { get; set; }
    private ProductId(Guid value)
    {
        Value = value;
    }
    public static ProductId CreateUnique()
    {
        return new ProductId(Guid.NewGuid());
    }

    public static ProductId Create(Guid value)
    {
        return new ProductId(value);
    }
}
