using Application.Orders.Queries.Get;
using Domain.IRepositories.UnitOfWorks;
using Domain.Orders.ValueObjects;
using Application.Orders.Queries.Share;
using Domain.Customers.ValueObjects;
using Domain.Exceptions;
using Domain.Orders;
using Domain.Orders.Entities;
using Domain.Products.ValueObjects;
using Domain.Shared.ValueObjects;

namespace Application.Test.Orders.Queries.Get;

public class GetOrderQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public GetOrderQueryHandlerTests()
    {
        _unitOfWorkMock = new();
    }

    [Fact]
    public async Task Handler_Should_ThrowOrderNotFoundException_WhenOrderIsNull()
    {
        // Arrange
        var handler = new GetOrderQueryHandler(_unitOfWorkMock.Object);
        var cancellationToken = It.IsAny<CancellationToken>();
        var orderId = OrderId.CreateUnique();

        var query = new GetOrderQuery(orderId.Value);

        _unitOfWorkMock.Setup(x => x.Queries.Order.GetByIdWithLineItemsMemoryCacheAsync(
                orderId,
                cancellationToken))
            .ReturnsAsync((Order?)null);

        // Act
        var act = async () => await handler.Handle(query, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<OrderNotFoundException>();

    }

    [Fact]
    public async Task Handler_Should_ReturnOrderResponse_WhenOrderIsNotNull()
    {
        // Arrange
        var handler = new GetOrderQueryHandler(_unitOfWorkMock.Object);
        var cancellationToken = It.IsAny<CancellationToken>();
        var orderId = OrderId.CreateUnique();
        var customerId = CustomerId.CreateUnique();
        var order = Order.Create(orderId, customerId);
        HashSet<LineItem> _lineItems = new()
        {
            LineItem.Create(
                LineItemId.CreateUnique(),
                orderId, 
                ProductId.CreateUnique(),
                new Money("USD", 100m))
        };

        order.Update(_lineItems);

        var orderResponse = new OrderResponse(orderId.Value, customerId.Value, order.LineItems);

        var query = new GetOrderQuery(orderId.Value);

        _unitOfWorkMock.Setup(x => x.Queries.Order.GetByIdWithLineItemsMemoryCacheAsync(
                orderId,
                cancellationToken))
            .ReturnsAsync(order);

        // Act
        var response = await handler.Handle(query, cancellationToken);

        // Assert
        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(orderResponse);
    }
}