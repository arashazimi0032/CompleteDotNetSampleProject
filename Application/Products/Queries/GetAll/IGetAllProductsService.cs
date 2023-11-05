using Application.Products.Queries.Share;

namespace Application.Products.Queries.GetAll;

public interface IGetAllProductsService
{
    Task<IEnumerable<ProductResponse>> GetAllProducts(CancellationToken cancellationToken = default);
}
