using Domain.Customers.ValueObjects;
using Domain.Exceptions;
using Domain.IRepositories.UnitOfWorks;
using Domain.Orders;
using Domain.Products.ValueObjects;
using Domain.Shared.ValueObjects;
using MediatR;
    
namespace Application.Orders.Commands.Create;

internal sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = Order.Create(CustomerId.Create(request.CustomerId));

        foreach (var productId in request.ProductId)
        {
            var product = await _unitOfWork.Queries.Product.GetByIdAsNoTrackAsync(ProductId.Create(productId), cancellationToken);

            if (product is null)
            {
                throw new ProductNotFoundException(productId);
            }

            var price = new Money(product.Price.Currency, product.Price.Amount);
            order.Add(ProductId.Create(productId), price);
        }

        await _unitOfWork.Commands.Order.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return order.Id.Value;
    }
}