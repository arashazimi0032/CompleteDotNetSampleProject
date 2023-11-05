using Domain.IRepositories;
using Domain.Products;

namespace Application.Products.Commands.Create;

public sealed class CreateProductService : ICreateProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task CreateProduct(string name, decimal price, CancellationToken cancellationToken = default)
    {
        var product = new Product()
        {
            Id = Guid.NewGuid(),
            Name = name, 
            Price = price
        };

        await _unitOfWork.Product.AddAsync(product, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}