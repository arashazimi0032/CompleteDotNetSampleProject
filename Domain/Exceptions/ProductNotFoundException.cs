namespace Domain.Exceptions;

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(Guid id)
        : base($"The product with the ID = {id} was not found!")
    {
    }
}