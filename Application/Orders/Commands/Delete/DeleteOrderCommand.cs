using MediatR;

namespace Application.Orders.Commands.Delete;

public record DeleteOrderCommand(Guid OrderId) : IRequest;
