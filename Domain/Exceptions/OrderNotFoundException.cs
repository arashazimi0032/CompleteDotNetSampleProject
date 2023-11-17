namespace Domain.Exceptions;

public class OrderNotFoundException : Exception
{
    public OrderNotFoundException(Guid orderId)
        : base($"The Order with the ID = {orderId} was not found!")
    {
    }
}