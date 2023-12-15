using Domain.Orders.Events;
using MediatR;

namespace Application.Orders.Events;

public sealed class OrderCreatedDomainEventHandler
    : INotificationHandler<OrderCreatedDomainEvent>
{
    public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine("Order Created!");
        await Task.CompletedTask;
    }
}