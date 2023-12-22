using Domain.Primitive.Models;

namespace Domain.Products.ValueObjects;

public record ProductId : ValueObject
{
    public Guid Value { get; set; }

    private ProductId()
    {

    }

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
