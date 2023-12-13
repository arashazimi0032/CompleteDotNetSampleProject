using Domain.Products;
using Domain.Shared.ValueObjects;

namespace Application.Products.Commands.Update;

public interface IUpdateProductService
{
    Task UpdateProduct(Guid id, string name, Money price, CancellationToken cancellationToken = default);
}