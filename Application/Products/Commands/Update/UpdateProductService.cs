using Domain.Exceptions;
using Domain.IRepositories;

namespace Application.Products.Commands.Update;

public sealed class UpdateProductService : IUpdateProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task UpdateProduct(Guid id, string name, decimal price, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.Product.GetByIdAsync(id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(id);
        }

        product.Name = name;
        product.Price = price;

        _unitOfWork.Product.Update(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}