using Domain.Primitive.Models;

namespace Domain.IRepositories.Queries.Caches;

public interface ICacheRepository<T, in TId>
    where T : Entity<TId>
    where TId : ValueObject
{
    Task<T?> GetByIdFromMemoryCacheAsync(TId id, CancellationToken cancellationToken = default);
}