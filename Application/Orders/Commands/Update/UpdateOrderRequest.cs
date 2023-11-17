namespace Application.Orders.Commands.Update;

public record UpdateOrderRequest(List<Guid> LineItemId, List<Guid> ProductId);
