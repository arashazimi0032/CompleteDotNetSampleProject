using Domain.Exceptions;
using Domain.IRepositories.UnitOfWorks;
using Domain.Orders.Entities;
using Domain.Orders.ValueObjects;
using Domain.Products.ValueObjects;
using Domain.Shared.ValueObjects;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Orders.Commands.Update;

internal sealed class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _memoryCache;

    public UpdateOrderCommandHandler(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    {
        _unitOfWork = unitOfWork;
        _memoryCache = memoryCache;
    }

    public async Task<Guid> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Create(command.OrderId);

        var orderKey = $"Order-{orderId}";

        var order = await _unitOfWork.Queries.Order.GetByIdWithLineItemsAsync(orderId, cancellationToken);

        var lineItems = new List<LineItem>();

        if (order is null)
        {
            throw new OrderNotFoundException(command.OrderId);
        }

        foreach (var productId in command.Request.ProductId)
        {
            var product = await _unitOfWork.Queries.Product.GetByIdAsync(ProductId.Create(productId), cancellationToken);

            if (product is null)
                throw new ProductNotFoundException(productId);

            var price = new Money(product.Price.Currency, product.Price.Amount);
            var lineItem = LineItem.Create(OrderId.Create(command.OrderId), ProductId.Create(productId), price);
            lineItems.Add(lineItem);
        }

        order.Update(lineItems);

        _unitOfWork.Commands.Order.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _memoryCache.Remove(orderKey);

        return order.Id.Value;
    }
}