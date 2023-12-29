using Application.Orders.Commands.Delete;
using Domain.Customers.ValueObjects;
using Domain.Exceptions;
using Domain.IRepositories.Commands;
using Domain.IRepositories.UnitOfWorks;
using Domain.Orders;
using Domain.Orders.ValueObjects;

namespace Application.Test.Orders.Commands.Delete;

public class DeleteOrderCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICommandUnitOfWork> _commandUnitOfWorkMock;
    private readonly Mock<IOrderCommandRepository> _orderCommandRepositoryMock;

    public DeleteOrderCommandHandlerTests()
    {
        _unitOfWorkMock = new();
        _commandUnitOfWorkMock = new();
        _orderCommandRepositoryMock = new();
    }

    [Fact]
    public async Task Handler_Should_ThrowOrderNotFoundException_WhenOrderIsNull()
    {
        // Arrange
        var handler = new DeleteOrderCommandHandler(_unitOfWorkMock.Object);
        var orderId = OrderId.CreateUnique();
        var cancellationToken = It.IsAny<CancellationToken>();

        var command = new DeleteOrderCommand(orderId.Value);

        _unitOfWorkMock.Setup(x => x.Queries.Order.GetByIdAsync(orderId, cancellationToken))
            .ReturnsAsync((Order?)null);
        
        // Act
        var act= async () => await handler.Handle(command, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<OrderNotFoundException>();
    }

    [Fact]
    public async Task Handler_Should_CallRemoveAndSaveChanges_WhenOrderIsNotNull()
    {
        // Arrange
        var handler = new DeleteOrderCommandHandler(_unitOfWorkMock.Object);
        var orderId = OrderId.CreateUnique();
        var cancellationToken = It.IsAny<CancellationToken>();
        var order = Order.Create(orderId, CustomerId.CreateUnique());

        var command = new DeleteOrderCommand(orderId.Value);

        _unitOfWorkMock.Setup(x => x.Queries.Order.GetByIdAsync(orderId, cancellationToken))
            .ReturnsAsync(order);

        _unitOfWorkMock.SetupGet(x => x.Commands).Returns(_commandUnitOfWorkMock.Object);
        _unitOfWorkMock.SetupGet(x => x.Commands.Order).Returns(_orderCommandRepositoryMock.Object);
        
        // Act
        var deletedOrderId = await handler.Handle(command, cancellationToken);

        // Assert
        _unitOfWorkMock.Verify(x => x.Commands.Order.Remove(order),
            Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handler_Should_ReturnGuidId_WhenOrderIsNotNull()
    {
        // Arrange
        var handler = new DeleteOrderCommandHandler(_unitOfWorkMock.Object);
        var orderId = OrderId.CreateUnique();
        var cancellationToken = It.IsAny<CancellationToken>();
        var order = Order.Create(orderId, CustomerId.CreateUnique());

        var command = new DeleteOrderCommand(orderId.Value);

        _unitOfWorkMock.Setup(x => x.Queries.Order.GetByIdAsync(orderId, cancellationToken))
            .ReturnsAsync(order);

        _unitOfWorkMock.SetupGet(x => x.Commands).Returns(_commandUnitOfWorkMock.Object);
        _unitOfWorkMock.SetupGet(x => x.Commands.Order).Returns(_orderCommandRepositoryMock.Object);
        
        // Act
        var deletedOrderId = await handler.Handle(command, cancellationToken);

        // Assert
        deletedOrderId.Should().NotBeEmpty();
        deletedOrderId.GetType().Should().Be(typeof(Guid));
    }
}