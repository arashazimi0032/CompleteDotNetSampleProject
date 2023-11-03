using Domain.IRepositories;
using Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Persistence.Repositories;

public sealed class OrderRepository : Repository<Order>, IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdWithLineItemsAsync(Guid id)
    {
        return await GetQueryable()
            .Include(o => o.LineItems)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
}