using Domain.IRepositories.Commands;
using Domain.Products;
using Domain.Products.ValueObjects;
using Domain.Shared.ValueObjects;
using infrastructure.Persistence;
using infrastructure.Persistence.Repositories.Commands;

namespace Infrastructure.Test.Persistence.Repositories.Commands;

public class CommandRepositoryTests
{
    private readonly ICommandRepository<Product, ProductId> _commandRepository;
    private readonly ApplicationDbContext _context;
    public CommandRepositoryTests()
    {
        _context = InMemoryDbContextGenerator.GetDbContext();

        _commandRepository = new CommandRepository<Product, ProductId>(_context);
    }

    [Fact]
    public async Task AddAsync_Should_Succeed()
    {
        // Arrange
        var price = new Money("USD", 100m);
        var product = Product.Create("Product", price);
        var cancellationToken = It.IsAny<CancellationToken>();

        // Act
        var act = async () => await _commandRepository.AddAsync(product, cancellationToken);

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }

    [Fact]
    public void Add_Should_Succeed()
    {
        // Arrange
        var price = new Money("USD", 100m);
        var product = Product.Create("Product", price);

        // Act
        var act = () =>  _commandRepository.Add(product);

        // Assert
        act.Should().NotThrow<Exception>();
    }

    [Fact]
    public void Update_Should_Succeed()
    {
        // Arrange
        var price = new Money("USD", 100m);
        var product = Product.Create("Product", price);

        // Act
        var act = () =>  _commandRepository.Update(product);

        // Assert
        act.Should().NotThrow<Exception>();
    }

    [Fact]
    public void Remove_Should_Succeed()
    {
        // Arrange
        var price = new Money("USD", 100m);
        var product = Product.Create("Product", price);

        // Act
        var act = () =>  _commandRepository.Remove(product);

        // Assert
        act.Should().NotThrow<Exception>();
    }

    [Fact]
    public void RemoveRange_Should_Succeed()
    {
        // Arrange
        var price1 = new Money("USD", 100m);
        var price2 = new Money("USD", 100m);
        var products = new List<Product>()
        {
            Product.Create("Product2", price1), 
            Product.Create("Product2", price2)
        };

        // Act
        var act = () =>  _commandRepository.RemoveRange(products);

        // Assert
        act.Should().NotThrow<Exception>();
    }
}