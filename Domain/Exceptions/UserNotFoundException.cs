namespace Domain.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(Guid userId) 
        : base($"The user with the ID = {userId} was not found!")
    {
    }
}