using Domain.Primitive.Events;

namespace Domain.Orders.Events;

public record OrderCreatedDomainEvent(Order Order) : IDomainEvent;
