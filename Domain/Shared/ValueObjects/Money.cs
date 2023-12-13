using Domain.Primitive.Models;

namespace Domain.Shared.ValueObjects;

public record Money(string Currency, decimal Amount) : ValueObject;
