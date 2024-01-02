using Application.Products.Commands.Create;
using Application.Products.Commands.Delete;
using Application.Products.Commands.Update;
using Application.Products.Queries.Get;
using Application.Products.Queries.GetAll;
using Application.Products.Queries.Share;
using Domain.Exceptions;
using Domain.Products;
using Domain.Products.ValueObjects;
using Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers;

namespace Presentation.Test.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<ICreateProductService> _createProductService;
    private readonly Mock<IDeleteProductService> _deleteProductService;
    private readonly Mock<IUpdateProductService> _updateProductService;
    private readonly Mock<IGetProductService> _getProductService;
    private readonly Mock<IGetAllProductsService> _getAllProductsService;
    private readonly ProductsController _productsController;

    public ProductsControllerTests()
    {
        _createProductService = new();
        _deleteProductService = new();
        _updateProductService = new();
        _getProductService = new();
        _getAllProductsService = new();

        _productsController = new ProductsController(
            _getAllProductsService.Object,
            _getProductService.Object,
            _updateProductService.Object,
            _deleteProductService.Object,
            _createProductService.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnOkResultWithListOfProducts_IfSucceed()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var products = new List<Product>() { Product.Create("Product", new Money("USD", 100m)) };
        var productResponse = products.Select(p => new ProductResponse(p.Id.Value, p.Name, p.Price)).ToList();

        _getAllProductsService.Setup(x => x.GetAllProducts(cancellationToken))
            .ReturnsAsync(productResponse);

        // Act
        var result = await _productsController.Get(cancellationToken);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeAssignableTo<IEnumerable<ProductResponse>>();
        var productResponses = okResult.Value as IEnumerable<ProductResponse>;
        productResponses.Should().HaveCount(1);
    }

    [Fact]
    public async Task Get_ShouldReturnNotFoundResult_IfProductNotFound()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var products = new List<Product>() { Product.Create("Product", new Money("USD", 100m)) };
        var productResponse = products.Select(p => new ProductResponse(p.Id.Value, p.Name, p.Price)).ToList();

        _getAllProductsService.Setup(x => x.GetAllProducts(cancellationToken))
            .ThrowsAsync(new ProductNotFoundException(products.First().Id.Value));

        // Act
        var result = await _productsController.Get(cancellationToken);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var resultObject = result as NotFoundObjectResult;
        resultObject.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        resultObject.Value.Should().Be($"The product with the ID = {products.First().Id.Value} was not found!");
    }

    [Fact]
    public async Task Get_ShouldReturnBadRequest_IfModelStateIsNotValid()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var products = new List<Product>() { Product.Create("Product", new Money("USD", 100m)) };
        var productResponse = products.Select(p => new ProductResponse(p.Id.Value, p.Name, p.Price)).ToList();

        _productsController.ModelState.AddModelError("Product", "Something went wrong");

        // Act
        var result = await _productsController.Get(cancellationToken);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var resultObject = result as BadRequestObjectResult;
        resultObject.Value.Should().Be("Some properties are not valid!");
        resultObject.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task GetById_ShouldReturnOkResultWithOneProductResponse_IfSucceed()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var product = Product.Create("Product", new Money("USD", 100m));
        var productResponse = new ProductResponse(product.Id.Value, product.Name, product.Price);

        _getProductService.Setup(x => x.GetProduct(product.Id.Value, cancellationToken))
            .ReturnsAsync(productResponse);

        // Act
        var result = await _productsController.Get(productResponse.Id, cancellationToken);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().Be(productResponse);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFoundResult_IfProductNotFound()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var product = Product.Create("Product", new Money("USD", 100m));
        var productResponse = new ProductResponse(product.Id.Value, product.Name, product.Price);

        _getProductService.Setup(x => x.GetProduct(product.Id.Value, cancellationToken))
            .ThrowsAsync(new ProductNotFoundException(productResponse.Id));

        // Act
        var result = await _productsController.Get(productResponse.Id, cancellationToken);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        notFoundResult.Value.Should().Be($"The product with the ID = {product.Id.Value} was not found!");
    }

    [Fact]
    public async Task GetById_ShouldReturnBadRequest_IfThrowAnyOtherException()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var product = Product.Create("Product", new Money("USD", 100m));
        var productResponse = new ProductResponse(product.Id.Value, product.Name, product.Price);

        _getProductService.Setup(x => x.GetProduct(product.Id.Value, cancellationToken))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _productsController.Get(productResponse.Id, cancellationToken);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var notFoundResult = result as BadRequestObjectResult;
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task GetById_ShouldReturnBadRequest_IfModelStateIsNotValid()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var products = new List<Product>() { Product.Create("Product", new Money("USD", 100m)) };
        var productResponse = products.Select(p => new ProductResponse(p.Id.Value, p.Name, p.Price)).ToList();

        _productsController.ModelState.AddModelError("Product", "Something went wrong");

        // Act
        var result = await _productsController.Get(cancellationToken);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var resultObject = result as BadRequestObjectResult;
        resultObject.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        resultObject.Value.Should().Be("Some properties are not valid!");
    }

    [Fact]
    public async Task Post_ShouldReturnOkResultWithOneProductId_IfSucceed()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var price = new Money("USD", 100m);
        var product = Product.Create("Product", price);

        _createProductService.Setup(x => x.CreateProduct("Product", price, cancellationToken))
            .ReturnsAsync(product.Id.Value);

        // Act
        var productIdResult = await _productsController.Post("Product", price, cancellationToken);

        // Assert
        productIdResult.Should().BeOfType<OkObjectResult>();
        var result = productIdResult as OkObjectResult;
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().Be(product.Id.Value);
    }

    [Fact]
    public async Task Post_ShouldReturnNotFoundResult_IfThrowAnException()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var price = new Money("USD", 100m);
        var product = Product.Create("Product", price);

        _createProductService.Setup(x => x.CreateProduct("Product", price, cancellationToken))
            .ThrowsAsync(new Exception());

        // Act
        var productIdResult = await _productsController.Post("Product", price, cancellationToken);

        // Assert
        productIdResult.Should().BeOfType<NotFoundObjectResult>();
        var result = productIdResult as NotFoundObjectResult;
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);;
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_IfModelStateIsNotValid()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var price = new Money("USD", 100m);
        var product = Product.Create("Product", price);

        _productsController.ModelState.AddModelError("Product", "Something went wrong");

        // Act
        var productIdResult = await _productsController.Post("Product", price, cancellationToken);

        // Assert
        productIdResult.Should().BeOfType<BadRequestObjectResult>();
        var result = productIdResult as BadRequestObjectResult;
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Value.Should().Be("Some properties are not valid!");
    }

    [Fact]
    public async Task Put_ShouldReturnOkResultWithProductId_IfSucceed()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var price = new Money("USD", 100m);
        var product = Product.Create("Product", price);
        var updateProductRequest = new UpdateProductRequest("Product Updated", price);

        _updateProductService.Setup(x => x.UpdateProduct(
                product.Id.Value,
                updateProductRequest.Name,
                updateProductRequest.Price, 
                cancellationToken))
            .ReturnsAsync(product.Id.Value);

        // Act
        var productIdResult = await _productsController.Put(product.Id.Value, updateProductRequest, cancellationToken);

        // Assert
        productIdResult.Should().BeOfType<OkObjectResult>();
        var result = productIdResult as OkObjectResult;
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().Be(product.Id.Value);
    }

    [Fact]
    public async Task Put_ShouldReturnNotFoundResult_IfThrowProductNotFoundException()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var price = new Money("USD", 100m);
        var product = Product.Create("Product", price);
        var updateProductRequest = new UpdateProductRequest("Product Updated", price);

        _updateProductService.Setup(x => x.UpdateProduct(
                product.Id.Value,
                updateProductRequest.Name,
                updateProductRequest.Price, 
                cancellationToken))
            .ThrowsAsync(new ProductNotFoundException(product.Id.Value));

        // Act
        var productIdResult = await _productsController.Put(product.Id.Value, updateProductRequest, cancellationToken);

        // Assert
        productIdResult.Should().BeOfType<NotFoundObjectResult>();
        var result = productIdResult as NotFoundObjectResult;
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Value.Should().Be($"The product with the ID = {product.Id.Value} was not found!");
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_IfModelStateIsNotValid()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var price = new Money("USD", 100m);
        var product = Product.Create("Product", price);
        var updateProductRequest = new UpdateProductRequest("Product Updated", price);

        _productsController.ModelState.AddModelError("Product", "Something went wrong");

        // Act
        var productIdResult = await _productsController.Put(product.Id.Value, updateProductRequest, cancellationToken);

        // Assert
        productIdResult.Should().BeOfType<BadRequestObjectResult>();
        var result = productIdResult as BadRequestObjectResult;
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task Delete_ShouldReturnOkResultWithProductId_IfSucceed()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var price = new Money("USD", 100m);
        var product = Product.Create("Product", price);

        _deleteProductService.Setup(x => x.DeleteProduct(product.Id.Value, cancellationToken))
            .ReturnsAsync(product.Id.Value);

        // Act
        var productIdResult = await _productsController.Delete(product.Id.Value, cancellationToken);

        // Assert
        productIdResult.Should().BeOfType<OkObjectResult>();
        var result = productIdResult as OkObjectResult;
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().Be(product.Id.Value);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFoundResult_IfThrowProductNotFoundException()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var price = new Money("USD", 100m);
        var product = Product.Create("Product", price);

        _deleteProductService.Setup(x => x.DeleteProduct(product.Id.Value, cancellationToken))
            .ThrowsAsync(new ProductNotFoundException(product.Id.Value));

        // Act
        var productIdResult = await _productsController.Delete(product.Id.Value, cancellationToken);

        // Assert
        productIdResult.Should().BeOfType<NotFoundObjectResult>();
        var result = productIdResult as NotFoundObjectResult;
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Value.Should().Be($"The product with the ID = {product.Id.Value} was not found!");
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequest_IfModelStateIsNotValid()
    {
        // Arrange
        var cancellationToken = It.IsAny<CancellationToken>();
        var price = new Money("USD", 100m);
        var product = Product.Create("Product", price);

        _productsController.ModelState.AddModelError("Product", "Something went wrong");

        // Act
        var productIdResult = await _productsController.Delete(product.Id.Value, cancellationToken);

        // Assert
        productIdResult.Should().BeOfType<BadRequestObjectResult>();
        var result = productIdResult as BadRequestObjectResult;
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }
}