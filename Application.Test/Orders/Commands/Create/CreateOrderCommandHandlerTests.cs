using Application.Orders.Commands.Create;
using Domain.Customers.ValueObjects;
using Domain.Exceptions;
using Domain.IRepositories.Commands;
using Domain.IRepositories.UnitOfWorks;
using Domain.Orders;
using Domain.Primitive.Result;
using Domain.Products;
using Domain.Products.ValueObjects;
using Domain.Shared.ValueObjects;
using Microsoft.CodeAnalysis;

namespace Application.Test.Orders.Commands.Create;

public class CreateOrderCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICommandUnitOfWork> _commandUnitOfWorkMock;
    private readonly Mock<IOrderCommandRepository> _OrderCommandRepositoryMock;

    public CreateOrderCommandHandlerTests()
    {
        _unitOfWorkMock = new();
        _commandUnitOfWorkMock = new();
        _OrderCommandRepositoryMock = new();
    }

    [Fact]
    public async Task Handler_Should_ReturnFailureResult_WhenProductIsNull()
    {
        // Arrange
        var createOrderHandler = new CreateOrderCommandHandler(_unitOfWorkMock.Object);
        var cancellationToken = It.IsAny<CancellationToken>();
        var customerId = CustomerId.CreateUnique();
        List<Guid> productIds = new()
        {
            Guid.NewGuid(),
            Guid.NewGuid()
        };

        var command = new CreateOrderCommand(customerId.Value, productIds);

        _unitOfWorkMock.Setup(x => x.Queries.Product.GetByIdAsNoTrackAsync(
            It.IsAny<ProductId>(), cancellationToken))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await createOrderHandler.Handle(command, cancellationToken);

        // Assert
        result.Should().BeOfType<Result<Guid>>();
        result.Error.Code.Should().Be("404");
        result.Error.Message.Should().Be($"The product with the ID = {productIds[0]} was not found!");
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handler_Should_CallAddAndSaveChanges_WhenProductIsNotNull()
    {
        // Arrange
        var createOrderHandler = new CreateOrderCommandHandler(_unitOfWorkMock.Object);
        var cancellationToken = It.IsAny<CancellationToken>();
        var customerId = CustomerId.CreateUnique();
        List<Guid> productIds = new()
        {
            Guid.NewGuid(),
            Guid.NewGuid()
        };

        var price = new Money("USD", 100m);
        var product = Product.Create("Product", price);

        var command = new CreateOrderCommand(customerId.Value, productIds);

        foreach (var productId in productIds)
        {
            _unitOfWorkMock.Setup(x => x.Queries.Product.GetByIdAsNoTrackAsync(
                    ProductId.Create(productId), cancellationToken))
                .ReturnsAsync(product);
        }

        _unitOfWorkMock.SetupGet(x => x.Commands).Returns(_commandUnitOfWorkMock.Object);
        _unitOfWorkMock.SetupGet(x => x.Commands.Order).Returns(_OrderCommandRepositoryMock.Object);

        // Act
        var createdOrderId = await createOrderHandler.Handle(command, cancellationToken);

        // Assert
        _unitOfWorkMock.Verify(x => x.Commands.Order.AddAsync(
            It.Is<Order>(o => o.Id.Value == createdOrderId.Value),
            cancellationToken), 
            Times.Once);

        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handler_Should_ReturnResultOfGuidId_WhenProductIsNotNull()
    {
        // Arrange
        var createOrderHandler = new CreateOrderCommandHandler(_unitOfWorkMock.Object);
        var cancellationToken = It.IsAny<CancellationToken>();
        var customerId = CustomerId.CreateUnique();
        List<Guid> productIds = new()
        {
            Guid.NewGuid(),
            Guid.NewGuid()
        };

        var price = new Money("USD", 100m);
        var product = Product.Create("Product", price);

        var command = new CreateOrderCommand(customerId.Value, productIds);

        foreach (var productId in productIds)
        {
            _unitOfWorkMock.Setup(x => x.Queries.Product.GetByIdAsNoTrackAsync(
                    ProductId.Create(productId), cancellationToken))
                .ReturnsAsync(product);
        }

        _unitOfWorkMock.SetupGet(x => x.Commands).Returns(_commandUnitOfWorkMock.Object);
        _unitOfWorkMock.SetupGet(x => x.Commands.Order).Returns(_OrderCommandRepositoryMock.Object);

        // Act
        var createdOrderId = await createOrderHandler.Handle(command, cancellationToken);

        // Assert
        createdOrderId.Value.Should().NotBeEmpty();
        createdOrderId.GetType().Should().Be(typeof(Result<Guid>));
    }
}