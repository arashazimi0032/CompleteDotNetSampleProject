using Application.Products.Queries.Share;
using Domain.IRepositories;

namespace Application.Products.Queries.GetAll;

public sealed class GetAllProductsService : IGetAllProductsService
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllProductsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ProductResponse>> GetAllProducts(CancellationToken cancellationToken = default)
    {
        var products = await _unitOfWork.Product.GetAllAsync(cancellationToken);

        return products
            .Select(p => new ProductResponse(p.Id.Value, p.Name, p.Price))
            .ToList();
    }
}