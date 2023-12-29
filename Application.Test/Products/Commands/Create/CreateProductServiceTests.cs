using Application.Products.Commands.Create;
using Domain.IRepositories.Commands;
using Domain.IRepositories.UnitOfWorks;
using Domain.Products;
using Domain.Shared.ValueObjects;

namespace Application.Test.Products.Commands.Create;

public class CreateProductServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICommandUnitOfWork> _commandUnitOfWorkMock;
    private readonly Mock<IProductCommandRepository> _productCommandRepositoryMock;

    public CreateProductServiceTests()
    {
        _unitOfWorkMock = new();
        _commandUnitOfWorkMock = new();
        _productCommandRepositoryMock = new();
    }

    [Fact]
    public async Task Service_Should_ReturnGuidId_WhenCreateSuccessfully()
    {
        // Arrange
        var createProductService = new CreateProductService(_unitOfWorkMock.Object);
        var cancellationToken = It.IsAny<CancellationToken>();
        var productName = "Product Name";
        var price = new Money("USD", 100m);
        var product = Product.Create(productName, price);

        _unitOfWorkMock.SetupGet(x => x.Commands).Returns(_commandUnitOfWorkMock.Object);
        _unitOfWorkMock.SetupGet(x => x.Commands.Product).Returns(_productCommandRepositoryMock.Object);

        // Act
        var createdProductId = await createProductService.CreateProduct(productName, price, cancellationToken);

        // Assert
        createdProductId.Should().NotBeEmpty();
        createdProductId.GetType().Should().Be(typeof(Guid));
    }

    [Fact]
    public async Task Service_Should_CallAddOnUnitOfWork()
    {
        // Arrange
        var createProductService = new CreateProductService(_unitOfWorkMock.Object);
        var cancellationToken = It.IsAny<CancellationToken>();
        var productName = "Product Name";
        var price = new Money("USD", 100m);
        var product = Product.Create(productName, price);

        _unitOfWorkMock.SetupGet(x => x.Commands).Returns(_commandUnitOfWorkMock.Object);
        _unitOfWorkMock.SetupGet(x => x.Commands.Product).Returns(_productCommandRepositoryMock.Object);

        // Act
        var createdProductId = await createProductService.CreateProduct(productName, price, cancellationToken);

        // Assert
        _unitOfWorkMock.Verify(
            x => x.Commands.Product.AddAsync(
                It.Is<Product>(p => p.Id.Value == createdProductId), 
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Service_Should_CallSaveChangesOne_WhenSuccessfullyCreate()
    {
        // Arrange
        var createProductService = new CreateProductService(_unitOfWorkMock.Object);
        var cancellationToken = It.IsAny<CancellationToken>();
        var productName = "Product Name";
        var price = new Money("USD", 100m);
        var product = Product.Create(productName, price);

        _unitOfWorkMock.SetupGet(x => x.Commands).Returns(_commandUnitOfWorkMock.Object);
        _unitOfWorkMock.SetupGet(x => x.Commands.Product).Returns(_productCommandRepositoryMock.Object);

        // Act
        var createdProductId = await createProductService.CreateProduct(productName, price, cancellationToken);

        // Assert
        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(cancellationToken),
            Times.Once);
    }
}