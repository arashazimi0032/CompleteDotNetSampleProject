using Application.Products.Commands.Create;
using Application.Products.Commands.Delete;
using Application.Products.Commands.Update;
using Application.Products.Queries.Get;
using Application.Products.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ICreateProductService _createProductService;
    private readonly IDeleteProductService _deleteProductService;
    private readonly IUpdateProductService _updateProductService;
    private readonly IGetProductService _getProductService;
    private readonly IGetAllProductsService _getAllProductsService;
    public ProductsController(
        IGetAllProductsService getAllProductsService, 
        IGetProductService getProductService, 
        IUpdateProductService updateProductService, 
        IDeleteProductService deleteProductService, 
        ICreateProductService createProductService)
    {
        _getAllProductsService = getAllProductsService;
        _getProductService = getProductService;
        _updateProductService = updateProductService;
        _deleteProductService = deleteProductService;
        _createProductService = createProductService;
    }

    // GET: api/<ProductsController>

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        var products = await _getAllProductsService.GetAllProducts(cancellationToken);
            
        return Ok(products);
    }

    // GET api/<ProductsController>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _getProductService.GetProduct(id, cancellationToken);

        return Ok(product);
    }

    // POST api/<ProductsController>
    [HttpPost]
    public async Task<IActionResult> Post(string name, decimal price, CancellationToken cancellationToken = default)
    {
        await _createProductService.CreateProduct(name, price, cancellationToken);
            
        return Ok();
    }

    // PUT api/<ProductsController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        await _updateProductService.UpdateProduct(id, request.Name, request.Price, cancellationToken);

        return NoContent();
    }

    // DELETE api/<ProductsController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        await _deleteProductService.DeleteProduct(id, cancellationToken);

        return NoContent();
    }
}