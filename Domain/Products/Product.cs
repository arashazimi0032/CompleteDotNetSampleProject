using Domain.Primitive;
using Domain.Shared;

namespace Domain.Products;

public class Product : Entity<ProductId>
{
    public Product(ProductId id) : base(id)
    {

    }
    private Product(ProductId id, string name, Money price) : base(id)
    {
        Name = name;
        Price = price;
    }
    public static Product Create(string name, Money price)
    {
        return new Product(ProductId.CreateUnique(), name, price);
    }

    public void Update(string name, Money price)
    {
        Name = name;
        Price = price;
    }
    public string Name { get; private set; }
    public Money Price { get; private set; }
}