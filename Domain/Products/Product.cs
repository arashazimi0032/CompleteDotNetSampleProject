using Domain.Primitive;
using Domain.Shared;

namespace Domain.Products;

public class Product : Entity
{
    private Product()
    {
    }
    public static Product Create(string name, Money price)
    {
        return new Product
        {
            Id = new ProductId(Guid.NewGuid()),
            Name = name,
            Price = price
        };
    }

    public void Update(string name, Money price)
    {
        Name = name;
        Price = price;
    }

    public ProductId Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Money Price { get; private set; }
}