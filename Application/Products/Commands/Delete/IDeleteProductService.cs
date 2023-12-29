using Domain.Products;

namespace Application.Products.Commands.Delete;

public interface IDeleteProductService
{
    Task<Guid> DeleteProduct(Guid id, CancellationToken cancellationToken = default);
}
