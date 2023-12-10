using Domain.IRepositories.Queries;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Persistence.Repositories.Queries;

public sealed class QueryUnitOfWork : IQueryUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IOrderQueryRepository Order { get; private set; }
    public ICustomerQueryRepository Customer { get; private set; }
    public IProductQueryRepository Product { get; private set; }
    public ILineItemQueryRepository LineItem { get; private set; }

    public QueryUnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Order = new OrderQueryRepository(_context);
        Customer = new CustomerQueryRepository(_context);
        Product = new ProductQueryRepository(_context);
        LineItem = new LineItemQueryRepository(_context);
    }
}