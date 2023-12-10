using Domain.Primitive;

namespace Domain.IRepositories.Queries;

public interface IQueryRepository<T, in TId>
    where T : Entity<TId>
    where TId : notnull
{
    IQueryable<T> GetQueryable();

    Task<IQueryable<T>> GetQueryableAsync();

    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
}
