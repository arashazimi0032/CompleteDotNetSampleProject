﻿using Domain.Orders.Entities;
using Domain.Orders.ValueObjects;

namespace Domain.IRepositories.Queries;

public interface ILineItemQueryRepository : IQueryRepository<LineItem, LineItemId>
{
}