using Domain.Primitive.Events;

namespace Domain.Primitive.Models;

public interface IHasDomainEvents
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }

    public void ClearDomainEvents();
}