using Domain.IRepositories.Queries;
using infrastructure.Persistence.Repositories.Queries;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Test.Persistence.Repositories.Queries;

public class QueryUnitOfWorkTests
{
    private readonly IQueryUnitOfWork _queryUnitOfWork;
    private readonly Mock<IMemoryCache> _memoryCacheMock;
    private readonly Mock<IDistributedCache> _distributedCacheMock;
    public QueryUnitOfWorkTests()
    {
        var _context = InMemoryDbContextGenerator.GetDbContext();
        _memoryCacheMock = new();
        _distributedCacheMock = new();

        _queryUnitOfWork = new QueryUnitOfWork(_context, _memoryCacheMock.Object, _distributedCacheMock.Object);
    }

    [Fact]
    public void QueryOrderUnitOfWork_ShouldBeOfType_OrderQueryRepository()
    {
        // Arrange

        // Act

        // Assert
        _queryUnitOfWork.Order.Should().BeAssignableTo<IOrderQueryRepository>();
        _queryUnitOfWork.Order.Should().NotBeNull();
    }

    [Fact]
    public void QueryCustomerUnitOfWork_ShouldBeOfType_CustomerQueryRepository()
    {
        // Arrange

        // Act

        // Assert
        _queryUnitOfWork.Customer.Should().BeAssignableTo<ICustomerQueryRepository>();
        _queryUnitOfWork.Customer.Should().NotBeNull();
    }

    [Fact]
    public void QueryProductUnitOfWork_ShouldBeOfType_ProductQueryRepository()
    {
        // Arrange

        // Act

        // Assert
        _queryUnitOfWork.Product.Should().BeAssignableTo<IProductQueryRepository>();
        _queryUnitOfWork.Product.Should().NotBeNull();
    }

    [Fact]
    public void QueryLineItemUnitOfWork_ShouldBeOfType_LineItemQueryRepository()
    {
        // Arrange

        // Act

        // Assert
        _queryUnitOfWork.LineItem.Should().BeAssignableTo<ILineItemQueryRepository>();
        _queryUnitOfWork.LineItem.Should().NotBeNull();
    }
}