using Domain.Products;

namespace Domain.IRepositories.Commands;

public interface IProductCommandRepository : ICommandRepository<Product, ProductId>
{
}