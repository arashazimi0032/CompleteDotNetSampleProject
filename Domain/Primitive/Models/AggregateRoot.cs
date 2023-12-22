namespace Domain.Primitive.Models;

public abstract class AggregateRoot<TId> : Entity<TId>
    where TId : ValueObject
{
    protected AggregateRoot()
    {

    }
    protected AggregateRoot(TId id) : base(id)
    {
    }
}