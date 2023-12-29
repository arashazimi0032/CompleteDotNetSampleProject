using Domain.Exceptions;
using Domain.IRepositories.UnitOfWorks;
using Domain.Products.ValueObjects;
using Domain.Shared.ValueObjects;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Products.Commands.Update;

public sealed class UpdateProductService : IUpdateProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _memoryCache;

    public UpdateProductService(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    {
        _unitOfWork = unitOfWork;
        _memoryCache = memoryCache;
    }

    public async Task<Guid> UpdateProduct(Guid id, string name, Money price, CancellationToken cancellationToken = default)
    {
        var productId = ProductId.Create(id);
        var key = $"Product-{productId}";
        var product = await _unitOfWork.Queries.Product.GetByIdAsync(productId, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(id);
        }

        product.Update(name, price);
        
        _unitOfWork.Commands.Product.Update(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _memoryCache.Remove(key);

        return productId.Value;
    }
}