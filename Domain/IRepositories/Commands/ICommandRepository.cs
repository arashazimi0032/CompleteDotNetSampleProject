using Domain.Primitive;

namespace Domain.IRepositories.Commands;

public interface ICommandRepository<in T, in TId> 
    where T : Entity<TId>
    where TId : notnull
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    void Add(T entity);

    void Update(T entity);

    void Remove(T entity);

    void RemoveRange(IEnumerable<T> entities);
}
