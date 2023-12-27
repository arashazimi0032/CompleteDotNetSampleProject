using Microsoft.EntityFrameworkCore;
using Domain.IRepositories.Queries;
using Domain.IRepositories.Queries.Caches;
using Domain.Primitive.Models;
using infrastructure.Common.NewtonSoftJson;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace infrastructure.Persistence.Repositories.Queries;

public class QueryRepository<T, TId> : IQueryRepository<T, TId>, ICacheRepository<T, TId>
    where T : Entity<TId>
    where TId : ValueObject
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCache _distributedCache;
    internal DbSet<T> dbSet;

    public QueryRepository(
        ApplicationDbContext context,
        IMemoryCache memoryCache,
        IDistributedCache distributedCache)
    {
        _context = context;
        _memoryCache = memoryCache;
        _distributedCache = distributedCache;
        dbSet = _context.Set<T>();
    }

    public IQueryable<T> GetQueryableAsNoTrack()
    {
        return GetQueryable().AsNoTracking();
    }

    public IQueryable<T> GetQueryable()
    {
        return dbSet;
    }

    public async Task<IQueryable<T>> GetQueryableAsNoTrackAsync()
    {
        return dbSet.AsNoTracking();
    }

    public async Task<IQueryable<T>> GetQueryableAsync()
    {
        return dbSet;
    }

    public async Task<IEnumerable<T>> GetAllAsNoTrackAsync(CancellationToken cancellationToken = default)
    {
        return await dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbSet.ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsNoTrackAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);

        _context.Entry(entity!).State = EntityState.Detached;
        return entity;
    }

    public async Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await dbSet.FindAsync(
            new object?[] { id, cancellationToken },
            cancellationToken: cancellationToken);
    }

    public async Task<T?> GetByIdFromMemoryCacheAsync(TId id, CancellationToken cancellationToken = default)
    {
        var key = $"{typeof(T).Name}-{id}";
        return await _memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(20));

                return GetByIdAsNoTrackAsync(id, cancellationToken);
            });
    }

    public async Task<T?> GetByIdFromRedisCacheAsync(TId id, CancellationToken cancellationToken = default)
    {
        var key = $"{typeof(T).Name}-{id}";

        var cachedEntity = await _distributedCache.GetStringAsync(key, cancellationToken);

        T? entity;
        if (string.IsNullOrEmpty(cachedEntity))
        {
            entity = await GetByIdAsNoTrackAsync(id, cancellationToken);

            if (entity is null) return entity;

            await _distributedCache.SetStringAsync(
                key,
                JsonConvert.SerializeObject(entity),
                cancellationToken);

            return entity;
        }

        entity = JsonConvert.DeserializeObject<T>(
            cachedEntity,
            new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = new PrivateResolver()
            });

        return entity;
    }
}