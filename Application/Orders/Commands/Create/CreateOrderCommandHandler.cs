using Domain.Customers;
using Domain.Exceptions;
using Domain.IRepositories;
using Domain.Orders;
using Domain.Products;
using Domain.Shared;
using MediatR;

namespace Application.Orders.Commands.Create;

internal sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = Order.Create(CustomerId.Create(request.CustomerId));

        foreach (var productId in request.ProductId)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(ProductId.Create(productId), cancellationToken);

            if (product is null)
            {
                throw new ProductNotFoundException(productId);
            }

            var price = new Money(product.Price.Currency, product.Price.Amount);
            order.Add(ProductId.Create(productId), price);
            
        }

        await _unitOfWork.Order.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}