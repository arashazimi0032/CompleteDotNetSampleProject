using Domain.IRepositories;
using Domain.Orders;

namespace infrastructure.Persistence.Repositories;

public sealed class LineItemRepository : Repository<LineItem>, ILineItemRepository
{
    private readonly ApplicationDbContext _context;

    public LineItemRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}