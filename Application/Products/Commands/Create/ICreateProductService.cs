using Domain.Shared.ValueObjects;

namespace Application.Products.Commands.Create;

public interface ICreateProductService
{
    Task<Guid> CreateProduct(string name, Money price, CancellationToken cancellationToken = default);
}