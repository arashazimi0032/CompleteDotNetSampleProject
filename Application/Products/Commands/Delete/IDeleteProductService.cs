namespace Application.Products.Commands.Delete;

public interface IDeleteProductService
{
    Task DeleteProduct(Guid id, CancellationToken cancellationToken = default);
}
