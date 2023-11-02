namespace Domain.Orders;

public class Order
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public HashSet<LineItem> LineItems { get; set; } = new();
}