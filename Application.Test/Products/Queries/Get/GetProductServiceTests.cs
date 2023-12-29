using Application.Products.Queries.Get;
using Application.Products.Queries.Share;
using Domain.Exceptions;
using Domain.IRepositories.UnitOfWorks;
using Domain.Products;
using Domain.Products.ValueObjects;
using Domain.Shared.ValueObjects;

namespace Application.Test.Products.Queries.Get;

public class GetProductServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public GetProductServiceTests()
    {
        _unitOfWorkMock = new();
    }

    [Fact]
    public async Task Service_Should_ThrowProductNotFoundException_WhenProductIsNull()
    {
        // Arrange
        var getProductService = new GetProductService(_unitOfWorkMock.Object);
        var productId = ProductId.CreateUnique();
        var cancellationToken = It.IsAny<CancellationToken>();

        _unitOfWorkMock.Setup(x => x.Queries.Product.GetByIdFromMemoryCacheAsync(
                productId,
                cancellationToken))
            .ReturnsAsync((Product?)null);

        // Act
        var act = async () => await getProductService.GetProduct(productId.Value, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<ProductNotFoundException>();
    }

    [Fact]
    public async Task Service_Should_ReturnProductResponse_WhenProductIsNotNull()
    {
        // Arrange
        var getProductService = new GetProductService(_unitOfWorkMock.Object);
        var productId = ProductId.CreateUnique();
        var cancellationToken = It.IsAny<CancellationToken>();
        var productName = "Product Name";
        var price = new Money("USD", 100m);
        var product = Product.Create(productName, price);
        var productResponse = new ProductResponse(productId.Value, productName, price);

        _unitOfWorkMock.Setup(x => x.Queries.Product.GetByIdFromMemoryCacheAsync(
                productId,
                cancellationToken))
            .ReturnsAsync(product);

        // Act
        var act = await getProductService.GetProduct(productId.Value, cancellationToken);

        // Assert
        act.Should().NotBeNull();
        act.Should().Be(productResponse);
    }
}