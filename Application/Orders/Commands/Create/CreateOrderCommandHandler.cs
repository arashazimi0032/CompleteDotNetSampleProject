using Domain.Customers.ValueObjects;
using Domain.Exceptions;
using Domain.IRepositories.UnitOfWorks;
using Domain.Orders;
using Domain.Primitive.Models;
using Domain.Primitive.Result;
using Domain.Products.ValueObjects;
using Domain.Shared.ValueObjects;
using MediatR;
    
namespace Application.Orders.Commands.Create;

internal sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = Order.Create(CustomerId.Create(request.CustomerId));

        foreach (var productId in request.ProductId)
        {
            var product = await _unitOfWork.Queries.Product.GetByIdAsNoTrackAsync(ProductId.Create(productId), cancellationToken);

            if (product is null)
            {
                //throw new ProductNotFoundException(productId);
                return Result.Failure<Guid>(
                    new Error("404",
                        $"The product with the ID = {productId} was not found!")
                    );
            }

            var price = new Money(product.Price.Currency, product.Price.Amount);
            order.Add(ProductId.Create(productId), price);
        }

        await _unitOfWork.Commands.Order.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(order.Id.Value);
    }
}