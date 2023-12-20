using Domain.IRepositories.Queries.Caches;
using Domain.Products;
using Domain.Products.ValueObjects;

namespace Domain.IRepositories.Queries;

public interface IProductQueryRepository
        : IQueryRepository<Product, ProductId>,
            ICacheRepository<Product, ProductId>
{
}