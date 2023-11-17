namespace Domain.Exceptions;

public class LineItemNotFoundException : Exception
{
    public LineItemNotFoundException(Guid lineItemId)
        : base($"The LineItem with the ID = {lineItemId} was not found!")
    {
    }
}