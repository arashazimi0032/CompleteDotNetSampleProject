using Application.Products.Queries.Share;

namespace Application.Products.Queries.Get;

public interface IGetProductService
{
    Task<ProductResponse> GetProduct(Guid id, CancellationToken cancellationToken = default);
}
