using Application.Orders.Queries.GetAll;
using Application.Orders.Queries.Share;
using Domain.Customers.ValueObjects;
using Domain.IRepositories.UnitOfWorks;
using Domain.Orders;

namespace Application.Test.Orders.Queries.GetAll;

public class GetAllOrdersQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public GetAllOrdersQueryHandlerTests()
    {
        _unitOfWorkMock = new();
    }

    [Fact]
    public async Task Handler_Should_ReturnListOfOrderResponses_WhenSuccess()
    {
        // Arrange
        var handler = new GetAllOrdersQueryHandler(_unitOfWorkMock.Object);
        var cancellationToken = It.IsAny<CancellationToken>();
        var query = new GetAllOrdersQuery();

        var orders = new List<Order>()
        {
            Order.Create(CustomerId.CreateUnique()),
            Order.Create(CustomerId.CreateUnique())
        };

        var orderResponses = orders.Select(o => new OrderResponse(
            o.Id.Value, o.CustomerId.Value, o.LineItems))
            .ToList();

        _unitOfWorkMock.Setup(x => x.Queries.Order.GetAllWithLineItemsAsNoTrackAsync(
            cancellationToken))
            .ReturnsAsync(orders);

        // Act
        var response = await handler.Handle(query, cancellationToken);

        // Assert
        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(orderResponses);
    }
}