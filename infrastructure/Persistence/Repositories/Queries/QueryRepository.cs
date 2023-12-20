using Microsoft.EntityFrameworkCore;
using Domain.IRepositories.Queries;
using Domain.IRepositories.Queries.Caches;
using Domain.Primitive.Models;
using Microsoft.Extensions.Caching.Memory;

namespace infrastructure.Persistence.Repositories.Queries;

public class QueryRepository<T, TId> : IQueryRepository<T, TId>, ICacheRepository<T, TId>
    where T : Entity<TId>
    where TId : ValueObject
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _memoryCache;
    internal DbSet<T> dbSet;

    public QueryRepository(ApplicationDbContext context, IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
        dbSet = _context.Set<T>();
    }

    public IQueryable<T> GetQueryable()
    {
        return dbSet.AsNoTracking();
    }

    public async Task<IQueryable<T>> GetQueryableAsync()
    {
        return dbSet.AsNoTracking();
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await dbSet.FindAsync(
                new object?[] { id, cancellationToken },
                cancellationToken: cancellationToken);

        _context.Entry(entity!).State = EntityState.Detached;
        return entity;
    }

    public async Task<T?> GetByIdFromMemoryCacheAsync(TId id, CancellationToken cancellationToken = default)
    {
        var key = $"{nameof(T)}-{id}";
        return await _memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

                return GetByIdAsync(id, cancellationToken);
            });
    }
}