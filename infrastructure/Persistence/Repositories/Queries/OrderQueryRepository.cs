using Domain.IRepositories.Queries;
using Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Persistence.Repositories.Queries;

public sealed class OrderQueryRepository : QueryRepository<Order, OrderId>, IOrderQueryRepository
{
    private readonly ApplicationDbContext _context;

    public OrderQueryRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdWithLineItemsAsync(OrderId id, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Include(o => o.LineItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Order?>> GetAllWithLineItemsAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Include(o => o.LineItems)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}