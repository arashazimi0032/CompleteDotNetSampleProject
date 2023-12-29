using Application.Orders.Commands.Update;
using Domain.Customers.ValueObjects;
using Domain.Exceptions;
using Domain.IRepositories.Commands;
using Domain.IRepositories.UnitOfWorks;
using Domain.Orders;
using Domain.Orders.Entities;
using Domain.Orders.ValueObjects;
using Domain.Products;
using Domain.Products.ValueObjects;
using Domain.Shared.ValueObjects;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Test.Orders.Commands.Update;

public class UpdateOrderCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICommandUnitOfWork> _commandUnitOfWorkMock;
    private readonly Mock<IOrderCommandRepository> _orderCommandRepositoryMock;
    private readonly Mock<IMemoryCache> _memoryCacheMock;

    public UpdateOrderCommandHandlerTests()
    {
        _unitOfWorkMock = new();
        _commandUnitOfWorkMock = new();
        _orderCommandRepositoryMock = new();
        _memoryCacheMock = new();
    }

    [Fact]
    public async Task Handle_Should_ThrowOrderNotFoundException_WhenOrderIsNull()
    {
        // Arrange 
        var handler = new UpdateOrderCommandHandler(_unitOfWorkMock.Object, _memoryCacheMock.Object);
        var orderId = OrderId.CreateUnique();
        var cancellationToken = It.IsAny<CancellationToken>();
        List<Guid> productIds = new()
        {
            Guid.NewGuid(),
            Guid.NewGuid()
        };

        var updateOrderRequest = new UpdateOrderRequest(productIds);

        var command = new UpdateOrderCommand(orderId.Value, updateOrderRequest);

        _unitOfWorkMock.Setup(x => x.Queries.Order.GetByIdWithLineItemsAsync(orderId, cancellationToken))
            .ReturnsAsync((Order?)null);

        _unitOfWorkMock.SetupGet(x => x.Commands).Returns(_commandUnitOfWorkMock.Object);
        _unitOfWorkMock.SetupGet(x => x.Commands.Order).Returns(_orderCommandRepositoryMock.Object);

        // Act
        var act = async () => await handler.Handle(command, cancellationToken);

        //Assert
        await act.Should().ThrowAsync<OrderNotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_ThrowProductNotFoundException_WhenOProductIsNull()
    {
        // Arrange 
        var handler = new UpdateOrderCommandHandler(_unitOfWorkMock.Object, _memoryCacheMock.Object);
        var orderId = OrderId.CreateUnique();
        var order = Order.Create(orderId, CustomerId.CreateUnique());
        var cancellationToken = It.IsAny<CancellationToken>();

        List<Guid> productIds = new()
        {
            Guid.NewGuid(),
            Guid.NewGuid()
        };

        var updateOrderRequest = new UpdateOrderRequest(productIds);

        var command = new UpdateOrderCommand(orderId.Value, updateOrderRequest);

        _unitOfWorkMock.Setup(x => x.Queries.Order.GetByIdWithLineItemsAsync(orderId, cancellationToken))
            .ReturnsAsync(order);

        foreach (var productId in productIds)
        {
            _unitOfWorkMock.Setup(x => x.Queries.Product.GetByIdAsync(
                    ProductId.Create(productId),
                    cancellationToken))
                .ReturnsAsync((Product?)null);
        }

        _unitOfWorkMock.SetupGet(x => x.Commands).Returns(_commandUnitOfWorkMock.Object);
        _unitOfWorkMock.SetupGet(x => x.Commands.Order).Returns(_orderCommandRepositoryMock.Object);

        // Act
        var act = async () => await handler.Handle(command, cancellationToken);

        //Assert
        await act.Should().ThrowAsync<ProductNotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_CallUpdateAndSaveChanges_WhenOrderAndProductAreNotNull()
    {
        // Arrange 
        var handler = new UpdateOrderCommandHandler(_unitOfWorkMock.Object, _memoryCacheMock.Object);
        var orderId = OrderId.CreateUnique();
        var order = Order.Create(orderId, CustomerId.CreateUnique());
        var price = new Money("USD", 100m);
        var product = Product.Create("Product", price);
        var lineItems = new List<LineItem>();
        var cancellationToken = It.IsAny<CancellationToken>();

        List<Guid> productIds = new()
        {
            Guid.NewGuid(),
            Guid.NewGuid()
        };

        var updateOrderRequest = new UpdateOrderRequest(productIds);

        var command = new UpdateOrderCommand(orderId.Value, updateOrderRequest);

        _unitOfWorkMock.Setup(x => x.Queries.Order.GetByIdWithLineItemsAsync(orderId, cancellationToken))
            .ReturnsAsync(order);

        foreach (var productId in productIds)
        {
            _unitOfWorkMock.Setup(x => x.Queries.Product.GetByIdAsync(
                    ProductId.Create(productId),
                    cancellationToken))
                .ReturnsAsync(product);

            lineItems.Add(LineItem.Create(orderId, ProductId.Create(productId), price));
        }

        order.Update(lineItems);

        _unitOfWorkMock.SetupGet(x => x.Commands).Returns(_commandUnitOfWorkMock.Object);
        _unitOfWorkMock.SetupGet(x => x.Commands.Order).Returns(_orderCommandRepositoryMock.Object);

        // Act
        var updatedOrderId= await handler.Handle(command, cancellationToken);

        //Assert
        _unitOfWorkMock.Verify(x => x.Commands.Order.Update(order), 
            Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), 
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_CallRemoveInMemoryCache_WhenOrderAndProductAreNotNull()
    {
        // Arrange 
        var handler = new UpdateOrderCommandHandler(_unitOfWorkMock.Object, _memoryCacheMock.Object);
        var orderId = OrderId.CreateUnique();
        var order = Order.Create(orderId, CustomerId.CreateUnique());
        var price = new Money("USD", 100m);
        var product = Product.Create("Product", price);
        var memoryCacheKey = $"Order-{orderId}";
        var cancellationToken = It.IsAny<CancellationToken>();

        List<Guid> productIds = new()
        {
            Guid.NewGuid(),
            Guid.NewGuid()
        };

        var updateOrderRequest = new UpdateOrderRequest(productIds);

        var command = new UpdateOrderCommand(orderId.Value, updateOrderRequest);

        _unitOfWorkMock.Setup(x => x.Queries.Order.GetByIdWithLineItemsAsync(orderId, cancellationToken))
            .ReturnsAsync(order);

        foreach (var productId in productIds)
        {
            _unitOfWorkMock.Setup(x => x.Queries.Product.GetByIdAsync(
                    ProductId.Create(productId),
                    cancellationToken))
                .ReturnsAsync(product);
        }

        _unitOfWorkMock.SetupGet(x => x.Commands).Returns(_commandUnitOfWorkMock.Object);
        _unitOfWorkMock.SetupGet(x => x.Commands.Order).Returns(_orderCommandRepositoryMock.Object);

        // Act
        var updatedOrderId= await handler.Handle(command, cancellationToken);

        //Assert
        _memoryCacheMock.Verify(x => x.Remove(memoryCacheKey), 
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnGuidId_WhenOrderAndProductAreNotNull()
    {
        // Arrange 
        var handler = new UpdateOrderCommandHandler(_unitOfWorkMock.Object, _memoryCacheMock.Object);
        var orderId = OrderId.CreateUnique();
        var order = Order.Create(orderId, CustomerId.CreateUnique());
        var price = new Money("USD", 100m);
        var product = Product.Create("Product", price);
        var cancellationToken = It.IsAny<CancellationToken>();

        List<Guid> productIds = new()
        {
            Guid.NewGuid(),
            Guid.NewGuid()
        };

        var updateOrderRequest = new UpdateOrderRequest(productIds);

        var command = new UpdateOrderCommand(orderId.Value, updateOrderRequest);

        _unitOfWorkMock.Setup(x => x.Queries.Order.GetByIdWithLineItemsAsync(orderId, cancellationToken))
            .ReturnsAsync(order);

        foreach (var productId in productIds)
        {
            _unitOfWorkMock.Setup(x => x.Queries.Product.GetByIdAsync(
                    ProductId.Create(productId),
                    cancellationToken))
                .ReturnsAsync(product);
        }

        _unitOfWorkMock.SetupGet(x => x.Commands).Returns(_commandUnitOfWorkMock.Object);
        _unitOfWorkMock.SetupGet(x => x.Commands.Order).Returns(_orderCommandRepositoryMock.Object);

        // Act
        var updatedOrderId= await handler.Handle(command, cancellationToken);

        //Assert
        updatedOrderId.Should().NotBeEmpty();
        updatedOrderId.GetType().Should().Be(typeof(Guid));
    }
}