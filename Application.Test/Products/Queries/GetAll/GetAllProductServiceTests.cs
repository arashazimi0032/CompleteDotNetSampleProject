using Application.Products.Queries.GetAll;
using Application.Products.Queries.Share;
using Domain.IRepositories.UnitOfWorks;
using Domain.Products;
using Domain.Shared.ValueObjects;

namespace Application.Test.Products.Queries.GetAll;

public class GetAllProductServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public GetAllProductServiceTests()
    {
        _unitOfWorkMock = new();
    }

    [Fact]
    public async Task Service_Should_ReturnListOfProductResponses_WhenSuccess()
    {
        // Arrange
        var getAllProductService = new GetAllProductsService(_unitOfWorkMock.Object);
        var cancellationToken = It.IsAny<CancellationToken>();

        List<Product> products = new()
        {
            Product.Create("Product 1", new Money("USD", 100m)),
            Product.Create("Product 2", new Money("USD", 150m))
        };

        var productResponse = products
            .Select(p => new ProductResponse(p.Id.Value, p.Name, p.Price))
            .ToList();

        _unitOfWorkMock
            .Setup(x => x.Queries.Product.GetAllAsNoTrackAsync(cancellationToken))
            .ReturnsAsync(products);

        // Act
        var response = await getAllProductService.GetAllProducts(cancellationToken);

        // Assert
        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(productResponse);

    }
}