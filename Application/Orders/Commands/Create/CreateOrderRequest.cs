using Domain.Products;

namespace Application.Orders.Commands.Create;

public record CreateOrderRequest(List<Guid> ProductId);
