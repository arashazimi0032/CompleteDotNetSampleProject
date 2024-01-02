using Domain.IRepositories.Commands;
using Domain.IRepositories.Queries;
using Domain.IRepositories.UnitOfWorks;
using Domain.Products;
using Domain.Products.ValueObjects;
using Domain.Shared.ValueObjects;
using infrastructure.Persistence;
using infrastructure.Persistence.Repositories.Commands;
using infrastructure.Persistence.Repositories.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Test.Persistence.Repositories.UnitOfWorks;

public class UnitOfWorkTests
{
    private readonly Mock<IMemoryCache> _memoryCacheMock;
    private readonly Mock<IDistributedCache> _distributedCacheMock; 
    private readonly ICommandRepository<Product, ProductId> _commandRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UnitOfWorkTests()
    {
        var context = InMemoryDbContextGenerator.GetDbContext();

        _memoryCacheMock = new();
        _distributedCacheMock = new();

        _commandRepository = new CommandRepository<Product, ProductId>(context);

        _unitOfWork = new UnitOfWork(context, _memoryCacheMock.Object, _distributedCacheMock.Object);
    }

    [Fact]
    public void Queries_ShouldBeOfType_QueryUnitOfWork()
    {
        // Arrange

        // Act

        // Assert
        _unitOfWork.Queries.Should().BeAssignableTo<IQueryUnitOfWork>();
        _unitOfWork.Queries.Should().NotBeNull();
    }

    [Fact]
    public void Commands_ShouldBeOfType_CommandUnitOfWork()
    {
        // Arrange

        // Act

        // Assert
        _unitOfWork.Commands.Should().BeAssignableTo<ICommandUnitOfWork>();
        _unitOfWork.Commands.Should().NotBeNull();
    }

    [Fact]
    public async Task SaveChangesAsync_Should_Succeed()
    {
        // Arrange
        var price = new Money("USD", 200m);
        var entity = Product.Create("Product", price);
        var cancellationToken = It.IsAny<CancellationToken>();

        await _commandRepository.AddAsync(entity, cancellationToken);
        
        // Act
        var act = async () => await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }

    [Fact]
    public void GetDbContext_Should_ReturnOfTypeDbContext()
    {
        // Arrange

        // Act
        var dbContext = _unitOfWork.GetDbContext();

        // Assert
        dbContext.Should().NotBeNull();
        dbContext.Should().BeOfType<ApplicationDbContext>();
    }
}