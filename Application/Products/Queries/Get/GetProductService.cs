using Application.Products.Queries.Share;
using Domain.Exceptions;
using Domain.IRepositories;
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
        var queryTask = await _unitOfWork.Product.GetQueryableAsync();

        var response = await queryTask
            .Where(p => p.Id == id)
            .Select(p => new ProductResponse(p.Id, p.Name, p.Price))
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
        {
            throw new ProductNotFoundException(id);
        }

        return response;
    }
}