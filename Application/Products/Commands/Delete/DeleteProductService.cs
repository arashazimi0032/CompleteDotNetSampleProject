using Domain.Exceptions;
using Domain.IRepositories;
using Domain.Products;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Application.Products.Commands.Delete;

public sealed class DeleteProductService : IDeleteProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task DeleteProduct(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.Product.GetByIdAsync(id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(id);
        }

        _unitOfWork.Product.Remove(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}