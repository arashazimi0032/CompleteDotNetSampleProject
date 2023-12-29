using Application.Products.Commands.Update;
using Domain.Exceptions;
using Domain.IRepositories.Commands;
using Domain.IRepositories.UnitOfWorks;
using Domain.Products;
using Domain.Products.ValueObjects;
using Domain.Shared.ValueObjects;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;

namespace Application.Test.Products.Commands.Update;

public class UpdateProductServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMemoryCache> _memoryCacheMock;
    private readonly Mock<ICommandUnitOfWork> _commandUnitOfWorkMock;
    private readonly Mock<IProductCommandRepository> _productCommandRepositoryMock;

    public UpdateProductServiceTests()
    {
        _unitOfWorkMock = new ();
        _memoryCacheMock = new();
        _commandUnitOfWorkMock = new();
        _productCommandRepositoryMock = new ();
    }

    [Fact]
    public async Task Service_Should_ThrowProductNotFoundException_WhenProductIsNull()
    {
        // Arrange
        var updateProductService = new UpdateProductService(_unitOfWorkMock.Object, _memoryCacheMock.Object);
        var productId = ProductId.CreateUnique();
        var price = new Money("USD", 100m);
        var cancellationToken = It.IsAny<CancellationToken>();

        _unitOfWorkMock.Setup(
                x => x.Queries.Product.GetByIdAsync(
                    productId,
                    cancellationToken))
            .ReturnsAsync((Product?)null);

        // Act
        var act = async () => await updateProductService.UpdateProduct(
            productId.Value,
            "testName", 
            price, 
            cancellationToken);

        // Assert
        await act.Should().ThrowAsync<ProductNotFoundException>();
    }

    [Fact]
    public async Task Service_Should_CallAddOnUnitOfWork_WhenProductIsNotNull()
    {
        // Arrange
        var guidValue = Guid.NewGuid();

        var updateProductService = new UpdateProductService(_unitOfWorkMock.Object, _memoryCacheMock.Object);

        var productId = ProductId.Create(guidValue);
        var price = new Money("USD", 100m);
        var product = Product.Create("New Product", price);
        var cancellationToken = It.IsAny<CancellationToken>();

        _unitOfWorkMock.Setup(
                x => x.Queries.Product.GetByIdAsync(
                    productId,
                    cancellationToken))
            .ReturnsAsync(product);

        _unitOfWorkMock.SetupGet(x => x.Commands).Returns(_commandUnitOfWorkMock.Object);
        _unitOfWorkMock.SetupGet(x => x.Commands.Product).Returns(_productCommandRepositoryMock.Object);

        // Act
        await updateProductService.UpdateProduct(
            productId.Value,
            "product Name Updated",
            price, 
            cancellationToken);

        // Assert
        _unitOfWorkMock.Verify(
            x => x.Commands.Product.Update(product),
            Times.Once);
        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Fact]
    public async Task Service_ShouldNotCallSaveChanges_WhenProductIsNull()
    {
        // Arrange
        var updateProductService = new UpdateProductService(_unitOfWorkMock.Object, _memoryCacheMock.Object);
        var productId = ProductId.CreateUnique();
        var price = new Money("USD", 100m);
        var cancellationToken = It.IsAny<CancellationToken>();

        _unitOfWorkMock.Setup(
                x => x.Queries.Product.GetByIdAsync(
                    productId,
                    cancellationToken))
            .ReturnsAsync((Product?)null);

        // Act
        var act = async () => await updateProductService.UpdateProduct(
            productId.Value,
            "testName",
            price,
            cancellationToken);

        // Assert
        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(cancellationToken),
            Times.Never);
    }

    [Fact]
    public async Task Service_Should_ReturnGuidId_WhenProductIsNotNull()
    {
        // Arrange
        var guidValue = Guid.NewGuid();

        var updateProductService = new UpdateProductService(_unitOfWorkMock.Object, _memoryCacheMock.Object);

        var productId = ProductId.Create(guidValue);
        var price = new Money("USD", 100m);
        var product = Product.Create("New Product", price);
        var cancellationToken = It.IsAny<CancellationToken>();

        _unitOfWorkMock.Setup(
                x => x.Queries.Product.GetByIdAsync(
                    productId,
                    cancellationToken))
            .ReturnsAsync(product);

        _unitOfWorkMock.SetupGet(x => x.Commands).Returns(_commandUnitOfWorkMock.Object);
        _unitOfWorkMock.SetupGet(x => x.Commands.Product).Returns(_productCommandRepositoryMock.Object);

        // Act
        var updatedProductId = await updateProductService.UpdateProduct(
            productId.Value,
            "product Name Updated",
            price,
            cancellationToken);

        // Assert
        updatedProductId.Should().NotBeEmpty();
        updatedProductId.GetType().Should().Be(typeof(Guid));
    }
}