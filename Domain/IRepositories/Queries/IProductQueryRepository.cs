using Domain.Products;

namespace Domain.IRepositories.Queries;

public interface IProductQueryRepository : IQueryRepository<Product, ProductId>
{
}