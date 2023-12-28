using Application.Products.Commands.Delete;
using Domain.Exceptions;
using Domain.IRepositories.Commands;
using Domain.IRepositories.UnitOfWorks;
using Domain.Products;
using Domain.Products.ValueObjects;
using Domain.Shared.ValueObjects;

namespace Application.Test.Products.Commands.Delete;

public class DeleteProductServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICommandUnitOfWork> _commandUnitOfWorkMock;
    private readonly Mock<IProductCommandRepository> _productCommandRepositoryMock;

    public DeleteProductServiceTests()
    {
        _unitOfWorkMock = new();
        _commandUnitOfWorkMock = new();
        _productCommandRepositoryMock = new();
    }

    [Fact]
    public async Task Service_Should_ThrowProductNotFoundException_WhenProductIsNull()
    {
        // Arrange
        var deleteProductService = new DeleteProductService(_unitOfWorkMock.Object);
        var productId = ProductId.CreateUnique();
        var cancellationToken = It.IsAny<CancellationToken>();

        _unitOfWorkMock.Setup(
                x => x.Queries.Product.GetByIdAsync(
                    productId,
                    cancellationToken))
            .ReturnsAsync((Product?)null);

        //Act
        var act = async () => await deleteProductService.DeleteProduct(productId.Value, cancellationToken);


        //Assert
        await act.Should().ThrowAsync<ProductNotFoundException>();
    }

    [Fact]
    public async Task Service_Should_CallRemoveOnUnitOfWorkCommandProduct_WhenProductIsNotNull()
    {
        // Arrange
        var deleteProductService = new DeleteProductService(_unitOfWorkMock.Object);
        var productId = ProductId.CreateUnique();
        var cancellationToken = It.IsAny<CancellationToken>();
        var price = new Money("USD", 100m);
        var product = Product.Create("Product", price);

        _unitOfWorkMock.Setup(
                x => x.Queries.Product.GetByIdAsync(
                    productId,
                    cancellationToken))
            .ReturnsAsync(product);

        _unitOfWorkMock.SetupGet(x => x.Commands).Returns(_commandUnitOfWorkMock.Object);
        _unitOfWorkMock.SetupGet(x => x.Commands.Product).Returns(_productCommandRepositoryMock.Object);

        //Act
        await deleteProductService.DeleteProduct(productId.Value, cancellationToken);


        //Assert
        _unitOfWorkMock.Verify(
            x => x.Commands.Product.Remove(product),
            Times.Once());
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Service_Should_NotCallSaveChanges_WhenProductIsNull()
    {
        // Arrange
        var deleteProductService = new DeleteProductService(_unitOfWorkMock.Object);
        var productId = ProductId.CreateUnique();
        var cancellationToken = It.IsAny<CancellationToken>();

        _unitOfWorkMock.Setup(
                x => x.Queries.Product.GetByIdAsync(
                    productId,
                    cancellationToken))
            .ReturnsAsync((Product?)null);

        //Act
        var act = async () => await deleteProductService.DeleteProduct(productId.Value, cancellationToken);


        //Assert
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Never);
    }
}