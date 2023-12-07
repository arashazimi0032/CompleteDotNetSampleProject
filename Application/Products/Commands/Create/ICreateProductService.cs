﻿using Domain.Shared;

namespace Application.Products.Commands.Create;

public interface ICreateProductService
{
    Task CreateProduct(string name, Money price, CancellationToken cancellationToken = default);
}