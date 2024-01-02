using Domain.Customers.ValueObjects;
using Domain.IRepositories.Commands;
using Domain.IRepositories.Queries;
using Domain.IRepositories.UnitOfWorks;
using Domain.Orders;
using Domain.Orders.Entities;
using Domain.Orders.ValueObjects;
using Domain.Products.ValueObjects;
using Domain.Shared.ValueObjects;
using infrastructure.Persistence;
using infrastructure.Persistence.Repositories.Commands;
using infrastructure.Persistence.Repositories.Queries;
using infrastructure.Persistence.Repositories.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Test.Persistence.Repositories.Queries;

public class OrderQueryRepositoryTests
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<IDistributedCache> _distributedCacheMock;
    private readonly IOrderQueryRepository _orderQueryRepository;
    private readonly ICommandRepository<Order, OrderId> _orderCommandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderQueryRepositoryTests()
    {
        _context = InMemoryDbContextGenerator.GetDbContext();
        var memoryCacheMock = new MemoryCache(new MemoryCacheOptions());
        _distributedCacheMock = new();
        _orderCommandRepository = new CommandRepository<Order, OrderId>(_context);
        _unitOfWork = new UnitOfWork(_context, memoryCacheMock, _distributedCacheMock.Object);

        _orderQueryRepository =
            new OrderQueryRepository(_context, memoryCacheMock, _distributedCacheMock.Object);
    }

    [Fact]
    public async Task GetByIdWithLineItemsAsNoTrackAsync_ShouldReturnOrder_WhenSucceed()
    {
        // Arrange
        var customerId = CustomerId.CreateUnique();
        var order = Order.Create(customerId);
        var productId = ProductId.CreateUnique();
        var price = new Money("USD", 100m);
        var lineItems = new List<LineItem>() { LineItem.Create(order.Id, productId, price) };
        order.Update(lineItems);
        var cancellationToken = It.IsAny<CancellationToken>();
        await _orderCommandRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Act
        var act = async () => await _orderQueryRepository.GetByIdWithLineItemsAsNoTrackAsync(
            It.IsAny<OrderId>(),
            cancellationToken);

        var retrievedOrder = await _orderQueryRepository.GetByIdWithLineItemsAsNoTrackAsync(
            order.Id,
            cancellationToken);

        // Assert

        await act.Should().NotThrowAsync<Exception>();
        retrievedOrder.Should().NotBeNull();
        retrievedOrder.Should().BeEquivalentTo(order);
        retrievedOrder.LineItems.Should().HaveCount(1);
        _context.Entry(retrievedOrder).State.Should().Be(EntityState.Detached);
    }

    [Fact]
    public async Task GetByIdWithLineItemsAsync_ShouldReturnOrder_WhenSucceed()
    {
        // Arrange
        var customerId = CustomerId.CreateUnique();
        var order = Order.Create(customerId);
        var productId = ProductId.CreateUnique();
        var price = new Money("USD", 100m);
        var lineItems = new List<LineItem>() { LineItem.Create(order.Id, productId, price) };
        order.Update(lineItems);
        var cancellationToken = It.IsAny<CancellationToken>();
        await _orderCommandRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Act
        var act = async () => await _orderQueryRepository.GetByIdWithLineItemsAsync(
            It.IsAny<OrderId>(),
            cancellationToken);

        var retrievedOrder = await _orderQueryRepository.GetByIdWithLineItemsAsync(
            order.Id,
            cancellationToken);

        // Assert

        await act.Should().NotThrowAsync<Exception>();
        retrievedOrder.Should().NotBeNull();
        retrievedOrder.Should().BeEquivalentTo(order);
        retrievedOrder.LineItems.Should().HaveCount(1);
        _context.Entry(retrievedOrder).State.Should().NotBe(EntityState.Detached);
    }

    [Fact]
    public async Task GetAllWithLineItemsAsNoTrackAsync_ShouldReturnOrder_WhenSucceed()
    {
        // Arrange
        var customerId = CustomerId.CreateUnique();
        var order = Order.Create(customerId);
        var productId = ProductId.CreateUnique();
        var price = new Money("USD", 100m);
        var lineItems = new List<LineItem>() { LineItem.Create(order.Id, productId, price) };
        order.Update(lineItems);
        var cancellationToken = It.IsAny<CancellationToken>();
        await _orderCommandRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Act
        var act = async () => await _orderQueryRepository.GetAllWithLineItemsAsNoTrackAsync(
            cancellationToken);

        var retrievedOrder = await _orderQueryRepository.GetAllWithLineItemsAsNoTrackAsync(
            cancellationToken);

        // Assert

        await act.Should().NotThrowAsync<Exception>();
        retrievedOrder.Should().NotBeNull();
        retrievedOrder.First().Should().BeEquivalentTo(order);
        retrievedOrder.First().LineItems.Should().HaveCount(1);
        _context.Entry(retrievedOrder.First()).State.Should().Be(EntityState.Detached);
    }

    [Fact]
    public async Task GetAllWithLineItemsAsync_ShouldReturnOrder_WhenSucceed()
    {
        // Arrange
        var customerId = CustomerId.CreateUnique();
        var order = Order.Create(customerId);
        var productId = ProductId.CreateUnique();
        var price = new Money("USD", 100m);
        var lineItems = new List<LineItem>() { LineItem.Create(order.Id, productId, price) };
        order.Update(lineItems);
        var cancellationToken = It.IsAny<CancellationToken>();
        await _orderCommandRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Act
        var act = async () => await _orderQueryRepository.GetAllWithLineItemsAsync(
            cancellationToken);

        var retrievedOrder = await _orderQueryRepository.GetAllWithLineItemsAsync(
            cancellationToken);

        // Assert

        await act.Should().NotThrowAsync<Exception>();
        retrievedOrder.Should().NotBeNull();
        retrievedOrder.First().Should().BeEquivalentTo(order);
        retrievedOrder.First().LineItems.Should().HaveCount(1);
        _context.Entry(retrievedOrder.First()).State.Should().NotBe(EntityState.Detached);
    }

    [Fact]
    public async Task GetByIdWithLineItemsMemoryCacheAsync_ShouldReturnOrder_WhenSucceed()
    {
        // Arrange
        var customerId = CustomerId.CreateUnique();
        var order = Order.Create(customerId);
        var productId = ProductId.CreateUnique();
        var price = new Money("USD", 100m);
        var lineItems = new List<LineItem>() { LineItem.Create(order.Id, productId, price) };
        order.Update(lineItems);
        var cancellationToken = It.IsAny<CancellationToken>();
        await _orderCommandRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Act
        var act = async () => await _orderQueryRepository.GetByIdWithLineItemsMemoryCacheAsync(
            order.Id,
            cancellationToken);

        var retrievedOrder = await _orderQueryRepository.GetByIdWithLineItemsMemoryCacheAsync(
            order.Id,
            cancellationToken);

        // Assert

        await act.Should().NotThrowAsync<Exception>();
        retrievedOrder.Should().NotBeNull();
        retrievedOrder.Should().BeEquivalentTo(order);
        retrievedOrder.LineItems.Should().HaveCount(1);
        _context.Entry(retrievedOrder).State.Should().Be(EntityState.Detached);
    }

    [Fact]
    public async Task GetByIdWithLineItemsRedisCacheAsync_ShouldReturnOrder_WhenSucceed()
    {
        // Arrange
        var customerId = CustomerId.CreateUnique();
        var order = Order.Create(customerId);
        var productId = ProductId.CreateUnique();
        var price = new Money("USD", 100m);
        var lineItems = new List<LineItem>() { LineItem.Create(order.Id, productId, price) };
        order.Update(lineItems);
        var cancellationToken = It.IsAny<CancellationToken>();
        await _orderCommandRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Act
        var act = async () => await _orderQueryRepository.GetByIdWithLineItemsRedisCacheAsync(
            order.Id,
            cancellationToken);

        var retrievedOrder = await _orderQueryRepository.GetByIdWithLineItemsRedisCacheAsync(
            order.Id,
            cancellationToken);

        // Assert

        await act.Should().NotThrowAsync<Exception>();
        retrievedOrder.Should().NotBeNull();
        retrievedOrder.Should().BeEquivalentTo(order);
        retrievedOrder.LineItems.Should().HaveCount(1);
        _context.Entry(retrievedOrder).State.Should().Be(EntityState.Detached);
    }
}