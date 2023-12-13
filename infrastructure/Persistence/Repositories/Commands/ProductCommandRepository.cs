using Domain.IRepositories.Commands;
using Domain.Products;
using Domain.Products.ValueObjects;

namespace infrastructure.Persistence.Repositories.Commands;

public sealed class ProductCommandRepository : CommandRepository<Product, ProductId>, IProductCommandRepository
{
    private readonly ApplicationDbContext _context;

    public ProductCommandRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}