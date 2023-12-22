using Domain.Exceptions;
using Domain.IRepositories.UnitOfWorks;
using Domain.Products.ValueObjects;
using Domain.Shared.ValueObjects;

namespace Application.Products.Commands.Update;

public sealed class UpdateProductService : IUpdateProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task UpdateProduct(Guid id, string name, Money price, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.Queries.Product.GetByIdAsNoTrackAsync(ProductId.Create(id), cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(id);
        }

        product.Update(name, price);
        
        _unitOfWork.Commands.Product.Update(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}