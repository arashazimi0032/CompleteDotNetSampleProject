namespace Domain.Exceptions;

public class CustomerNotFoundException : Exception
{
    public CustomerNotFoundException(Guid id)
        : base($"The customer with the ID = {id} was not found!")
    {
    }
}