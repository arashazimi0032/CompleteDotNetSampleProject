using Application.Products.Queries.Share;
using Domain.Exceptions;
using Domain.IRepositories;
using Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Queries.Get;

public sealed class GetProductService : IGetProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductResponse> GetProduct(Guid id, CancellationToken cancellationToken = default)
    {

        var product = await _unitOfWork.Product.GetByIdAsync(ProductId.Create(id), cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(id);
        }

        var response = new ProductResponse(id, product.Name, product.Price);

        return response;
    }
}