using Domain.IRepositories.Queries;
using Domain.Orders;

namespace infrastructure.Persistence.Repositories.Queries;

public sealed class LineItemQueryRepository : QueryRepository<LineItem, LineItemId>, ILineItemQueryRepository
{
    private readonly ApplicationDbContext _context;

    public LineItemQueryRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}