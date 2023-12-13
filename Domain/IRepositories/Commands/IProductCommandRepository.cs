using Domain.Products;
using Domain.Products.ValueObjects;

namespace Domain.IRepositories.Commands;

public interface IProductCommandRepository : ICommandRepository<Product, ProductId>
{
}