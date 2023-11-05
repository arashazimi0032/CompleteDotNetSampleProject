namespace Application.Products.Commands.Update;

public interface IUpdateProductService
{
    Task UpdateProduct(Guid id, string name, decimal price, CancellationToken cancellationToken = default);
}