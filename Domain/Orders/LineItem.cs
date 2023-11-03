using Domain.Primitive;

namespace Domain.Orders;

public class LineItem : Entity
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public decimal Price { get; set; }
}