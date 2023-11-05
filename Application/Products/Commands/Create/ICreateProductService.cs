namespace Application.Products.Commands.Create;

public interface ICreateProductService
{
    Task CreateProduct(string name, decimal price, CancellationToken cancellationToken = default);
}