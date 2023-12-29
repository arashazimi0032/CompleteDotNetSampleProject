using MediatR;

namespace Application.Orders.Commands.Update;

public record UpdateOrderCommand(Guid OrderId, UpdateOrderRequest Request) : IRequest<Guid>;
