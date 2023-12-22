using Domain.Primitive.Models;

namespace Domain.IRepositories.Queries.Caches;

public interface ICacheRepository<T, in TId>
    where T : Entity<TId>
    where TId : ValueObject
{
    Task<T?> GetByIdFromMemoryCacheAsNoTrackAsync(TId id, CancellationToken cancellationToken = default);
    Task<T?> GetByIdFromMemoryCacheAsync(TId id, CancellationToken cancellationToken = default);

    Task<T?> GetByIdFromRedisCacheAsNoTrackAsync(TId id, CancellationToken cancellationToken = default);
    Task<T?> GetByIdFromRedisCacheAsync(TId id, CancellationToken cancellationToken = default);
}