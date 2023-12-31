﻿using Domain.Primitive.Result;
using MediatR;

namespace Application.Orders.Commands.Create;

public record CreateOrderCommand(Guid CustomerId, List<Guid> ProductId) : IRequest<Result<Guid>>;
