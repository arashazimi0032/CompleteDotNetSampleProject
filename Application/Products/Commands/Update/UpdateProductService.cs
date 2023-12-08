using Domain.Exceptions;
using Domain.IRepositories;
using Domain.Products;
using Domain.Shared;

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
        var product = await _unitOfWork.Product.GetByIdAsync(ProductId.Create(id), cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(id);
        }

        product.Update(name, price);
        
        _unitOfWork.Product.Update(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}