using Domain.Primitive;

namespace Domain.Orders;

public class Order : Entity
{
    public Guid CustomerId { get; set; }
    public HashSet<LineItem> LineItems { get; set; } = new();
}