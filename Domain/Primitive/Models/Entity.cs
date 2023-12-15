using Domain.Primitive.Events;

namespace Domain.Primitive.Models;

public abstract class Entity<TId> : IEquatable<Entity<TId>>, IHasDomainEvents
    where TId : ValueObject
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public TId Id { get; protected set; }

    protected Entity(TId id)
    {
        Id = id;
    }

    protected void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public bool Equals(Entity<TId>? other)
    {
        return Equals((object?)other);
    }

    public override bool Equals(object? obj)
    {
        return obj is Entity<TId> entity && Id.Equals(entity.Id);
    }

    public static bool operator ==(Entity<TId> left, Entity<TId> right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity<TId> left, Entity<TId> right)
    {
        return !Equals(left, right);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}