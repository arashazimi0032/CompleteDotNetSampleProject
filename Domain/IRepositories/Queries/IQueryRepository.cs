using Domain.IRepositories.Queries.Caches;
using Domain.Primitive.Models;

namespace Domain.IRepositories.Queries;

public interface IQueryRepository<T, in TId>
    where T : Entity<TId>
    where TId : ValueObject
{
    IQueryable<T> GetQueryableAsNoTrack();
    IQueryable<T> GetQueryable();

    Task<IQueryable<T>> GetQueryableAsNoTrackAsync();
    Task<IQueryable<T>> GetQueryableAsync();

    Task<IEnumerable<T>> GetAllAsNoTrackAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsNoTrackAsync(TId id, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
}
