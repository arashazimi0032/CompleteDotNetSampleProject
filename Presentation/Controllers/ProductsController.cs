﻿using Application.Products.Commands.Create;
using Application.Products.Commands.Delete;
using Application.Products.Commands.Update;
using Application.Products.Queries.Get;
using Application.Products.Queries.GetAll;
using Domain.Attributes;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Shared.ValueObjects;
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
        if (!ModelState.IsValid)
        {
            return BadRequest("Some properties are not valid!");
        }

        try
        {
            var products = await _getAllProductsService.GetAllProducts(cancellationToken);
            
            return Ok(products);
        }
        catch (ProductNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    // GET api/<ProductsController>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Some properties are not valid!");
        }

        try
        {
            var product = await _getProductService.GetProduct(id, cancellationToken);

            return Ok(product);
        }
        catch (ProductNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // POST api/<ProductsController>
    [HttpPost]
    //[CustomAuthorize(Role.Admin)]
    [CustomAuthorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Post(string name, Money price, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Some properties are not valid!");
        }

        try
        {
            var productId = await _createProductService.CreateProduct(name, price, cancellationToken);
            
            return Ok(productId);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    // PUT api/<ProductsController>/5
    [HttpPut("{id}")]
    [CustomAuthorize(Role.Admin)]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Some properties are not valid!");
        }

        try
        {
            var productId = await _updateProductService.UpdateProduct(id, request.Name, request.Price, cancellationToken);

            return Ok(productId);
        }
        catch (ProductNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    // DELETE api/<ProductsController>/5
    [HttpDelete("{id}")]
    [CustomAuthorize(Role.Admin)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Some properties are not valid!");
        }

        try
        {
            var productId = await _deleteProductService.DeleteProduct(id, cancellationToken);

            return Ok(productId);
        }
        catch (ProductNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}