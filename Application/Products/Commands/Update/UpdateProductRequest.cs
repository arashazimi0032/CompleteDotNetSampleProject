using Domain.Shared;

namespace Application.Products.Commands.Update;

public record UpdateProductRequest(string Name, Money Price);
