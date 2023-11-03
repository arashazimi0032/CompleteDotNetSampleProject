using Domain.Primitive;

namespace Domain.Products;

public class Product : Entity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}