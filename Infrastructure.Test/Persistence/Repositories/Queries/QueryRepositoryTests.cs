using System.Collections;
using Domain.IRepositories.Commands;
using Domain.IRepositories.Queries;
using Domain.IRepositories.Queries.Caches;
using Domain.IRepositories.UnitOfWorks;
using Domain.Products.ValueObjects;
using Domain.Products;
using Domain.Shared.ValueObjects;
using infrastructure.Persistence.Repositories.Commands;
using infrastructure.Persistence.Repositories.Queries;
using infrastructure.Persistence.Repositories.UnitOfWorks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Moq;

namespace Infrastructure.Test.Persistence.Repositories.Queries;

public class QueryRepositoryTests
{
    private readonly IQueryRepository<Product, ProductId> _queryRepository;
    private readonly ICacheRepository<Product, ProductId> _cacheRepository;
    private readonly ICommandRepository<Product, ProductId> _commandRepository;
    private readonly Mock<IDistributedCache> _distributedCacheMock;
    private readonly IUnitOfWork _unitOfWork;

    public QueryRepositoryTests()
    {
        var context = InMemoryDbContextGenerator.GetDbContext();

        IMemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
        _distributedCacheMock = new();

        _queryRepository = new QueryRepository<Product, ProductId>(
            context, 
            memoryCache, 
            _distributedCacheMock.Object);

        _cacheRepository = new QueryRepository<Product, ProductId>(
            context, 
            memoryCache, 
            _distributedCacheMock.Object);

        _commandRepository = new CommandRepository<Product, ProductId>(context);
        _unitOfWork = new UnitOfWork(context, memoryCache, _distributedCacheMock.Object);
    }

    [Fact]
    public void GetQueryableAsNoTrack_Should_Succeed()
    {
        // Arrange

        // Act
        var act = () => _queryRepository.GetQueryableAsNoTrack();

        // Assert
        act.Should().NotThrow<Exception>();
    }

    [Fact]
    public void GetQueryableAsNoTrack_Should_ReturnQueryableOfTypeProduct()
    {
        // Arrange

        // Act
        var queryable = _queryRepository.GetQueryableAsNoTrack();

        // Assert
        queryable.Should().BeAssignableTo<IQueryable>();
        queryable.Should().BeOfType<EntityQueryable<Product>>();
    }

    [Fact]
    public void GetQueryable_Should_Succeed()
    {
        // Arrange

        // Act
        var act = () => _queryRepository.GetQueryable();

        // Assert
        act.Should().NotThrow<Exception>();
    }

    [Fact]
    public void GetQueryable_Should_ReturnQueryableOfTypeProduct()
    {
        // Arrange

        // Act
        var queryable = _queryRepository.GetQueryable();

        // Assert
        queryable.Should().BeAssignableTo<IQueryable>();
        queryable.Should().BeOfType<InternalDbSet<Product>>();
    }

    [Fact]
    public async Task GetQueryableAsNoTrackAsync_Should_Succeed()
    {
        // Arrange

        // Act
        var act = async () => await _queryRepository.GetQueryableAsNoTrackAsync();

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }

    [Fact]
    public async Task GetQueryableAsNoTrackAsync_Should_ReturnQueryableOfTypeProduct()
    {
        // Arrange

        // Act
        var queryable = await _queryRepository.GetQueryableAsNoTrackAsync();

        // Assert
        queryable.Should().BeAssignableTo<IQueryable>();
        queryable.Should().BeOfType<EntityQueryable<Product>>();
    }

    [Fact]
    public async Task GetQueryableAsync_Should_Succeed()
    {
        // Arrange

        // Act
        var act = async () => await  _queryRepository.GetQueryableAsync();

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }

    [Fact]
    public async Task GetQueryableAsync_Should_ReturnQueryableOfTypeProduct()
    {
        // Arrange

        // Act
        var queryable = await _queryRepository.GetQueryableAsync();

        // Assert
        queryable.Should().BeAssignableTo<IQueryable>();
        queryable.Should().BeOfType<InternalDbSet<Product>>();
    }

    [Fact]
    public async Task GetAllAsNoTrackAsync_Should_Succeed()
    {
        // Arrange

        // Act
        var act = async () => await _queryRepository.GetAllAsNoTrackAsync();

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }

    [Fact]
    public async Task GetAllAsNoTrackAsync_Should_ReturnIEnumerableOfTypeProduct()
    {
        // Arrange
        var product = Product.Create("Product", new Money("USD", 100m));
        var cancellationToken = It.IsAny<CancellationToken>();
        await _commandRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Act
        var entities = await _queryRepository.GetAllAsNoTrackAsync(cancellationToken);

        // Assert
        entities.Should().BeAssignableTo<IEnumerable>();
        entities.Should().BeOfType<List<Product>>();
        entities.Should().HaveCount(1);
        entities.First().Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task GetAllAsync_Should_Succeed()
        
    {
        // Arrange

        // Act
        var act = async () => await _queryRepository.GetAllAsync();

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }

    [Fact]
    public async Task GetAllAsync_Should_ReturnIEnumerableOfTypeProduct()
    {
        // Arrange
        var product = Product.Create("Product", new Money("USD", 100m));
        var cancellationToken = It.IsAny<CancellationToken>();
        await _commandRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Act
        var entities = await _queryRepository.GetAllAsync(cancellationToken);

        // Assert
        entities.Should().BeAssignableTo<IEnumerable>();
        entities.Should().BeOfType<List<Product>>();
        entities.Should().HaveCount(1);
        entities.First().Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task GetByIdAsNoTrackAsync_Should_Succeed()
    {
        // Arrange
        var product = Product.Create("Product", new Money("USD", 100m));
        var cancellationToken = It.IsAny<CancellationToken>();
        await _commandRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Act
        var act = async () => await _queryRepository.GetByIdAsNoTrackAsync(product.Id, cancellationToken);

        // Assert
        await act.Should().NotThrowAsync<Exception>();

    }

    [Fact]
    public async Task GetByIdAsNoTrackAsync_Should_ReturnProduct()
    {
        // Arrange
        var product = Product.Create("Product", new Money("USD", 100m));
        var cancellationToken = It.IsAny<CancellationToken>();
        await _commandRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Act
        var retrievedProduct = await _queryRepository.GetByIdAsNoTrackAsync(product.Id, cancellationToken);

        // Assert
        retrievedProduct.Should().NotBeNull();
        retrievedProduct.Should().BeEquivalentTo(product);

    }

    [Fact]
    public async Task GetByIdAsync_Should_Succeed()
    {
        // Arrange
        var product = Product.Create("Product", new Money("USD", 100m));
        var cancellationToken = It.IsAny<CancellationToken>();
        await _commandRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Act
        var act = async () => await _queryRepository.GetByIdAsync(product.Id, cancellationToken);

        // Assert
        await act.Should().NotThrowAsync<Exception>();

    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnProduct()
    {
        // Arrange
        var product = Product.Create("Product", new Money("USD", 100m));
        var cancellationToken = It.IsAny<CancellationToken>();
        await _commandRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Act
        var retrievedProduct = await _queryRepository.GetByIdAsync(product.Id, cancellationToken);

        // Assert
        retrievedProduct.Should().NotBeNull();
        retrievedProduct.Should().BeEquivalentTo(product);

    }

    [Fact]
    public async Task GetByIdFromMemoryCacheAsync_Should_Succeed()
    {
        // Arrange
        var product = Product.Create("Product", new Money("USD", 100m));
        var cancellationToken = It.IsAny<CancellationToken>();
        await _commandRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Act
        var act = async () => await _cacheRepository.GetByIdFromMemoryCacheAsync(product.Id, cancellationToken);

        // Assert
        await act.Should().NotThrowAsync<Exception>();

    }

    [Fact]
    public async Task GetByIdFromMemoryCacheAsync_Should_ReturnProduct()
    {
        // Arrange
        var product = Product.Create("Product", new Money("USD", 100m));
        var cancellationToken = It.IsAny<CancellationToken>();
        await _commandRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Act
        var retrievedProduct = await _cacheRepository.GetByIdFromMemoryCacheAsync(product.Id, cancellationToken);

        // Assert
        retrievedProduct.Should().NotBeNull();
        retrievedProduct.Should().BeEquivalentTo(product);

    }

    [Fact]
    public async Task GetByIdFromRedisCacheAsync_Should_Succeed()
    {
        // Arrange
        var product = Product.Create("Product", new Money("USD", 100m));
        var cancellationToken = It.IsAny<CancellationToken>();
        await _commandRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Act
        var act = async () => await _cacheRepository.GetByIdFromRedisCacheAsync(product.Id, cancellationToken);

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }

    [Fact]
    public async Task GetByIdFromRedisCacheAsync_Should_ReturnProduct()
    {
        // Arrange
        var product = Product.Create("Product", new Money("USD", 100m));
        var cancellationToken = It.IsAny<CancellationToken>();
        await _commandRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Act
        var retrievedProduct = await _cacheRepository.GetByIdFromRedisCacheAsync(product.Id, cancellationToken);

        // Assert
        retrievedProduct.Should().NotBeNull();
        retrievedProduct.Should().BeEquivalentTo(product);
    }
}