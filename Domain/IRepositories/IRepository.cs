using Domain.Primitive;

namespace Domain.IRepositories;

public interface IRepository<T, in TId> 
    where T : Entity<TId>
    where TId : notnull
{
    IQueryable<T> GetQueryable();
    
    Task<IQueryable<T>> GetQueryableAsync();

    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    void Add(T entity);

    void Update(T entity);

    void Remove(T entity);

    void RemoveRange(IEnumerable<T> entities);
}
