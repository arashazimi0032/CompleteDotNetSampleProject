using Domain.IRepositories.Queries;
using Domain.Products;
using Domain.Products.ValueObjects;

namespace infrastructure.Persistence.Repositories.Queries;

public sealed class ProductQueryRepository : QueryRepository<Product, ProductId>, IProductQueryRepository
{
    private readonly ApplicationDbContext _context;

    public ProductQueryRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}