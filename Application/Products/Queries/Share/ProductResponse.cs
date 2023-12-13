using Domain.Shared.ValueObjects;

namespace Application.Products.Queries.Share;

public record ProductResponse(Guid Id, string Name, Money Price);
