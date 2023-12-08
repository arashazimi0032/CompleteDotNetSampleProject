using Domain.Products;

namespace Domain.IRepositories;

public interface IProductRepository : IRepository<Product, ProductId>
{
}