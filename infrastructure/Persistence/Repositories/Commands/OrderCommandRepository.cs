﻿using Domain.IRepositories.Commands;
using Domain.Orders;

namespace infrastructure.Persistence.Repositories.Commands;

public sealed class OrderCommandRepository : CommandRepository<Order, OrderId>, IOrderCommandRepository
{
    private readonly ApplicationDbContext _context;

    public OrderCommandRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}