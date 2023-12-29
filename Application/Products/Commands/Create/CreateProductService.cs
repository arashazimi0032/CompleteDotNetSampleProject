using Domain.IRepositories.UnitOfWorks;
using Domain.Products;
using Domain.Shared.ValueObjects;

namespace Application.Products.Commands.Create;

public sealed class CreateProductService : ICreateProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateProduct(string name, Money price, CancellationToken cancellationToken = default)
    {
        var product = Product.Create(
            name, 
            price);

        await _unitOfWork.Commands.Product.AddAsync(product, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product.Id.Value;
    }
}