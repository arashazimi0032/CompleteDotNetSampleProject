using Domain.Orders;
using Domain.Products;

namespace Application.Orders.Commands.Update;

public record UpdateOrderRequest(List<Guid> ProductId);
