namespace Events.Domain.Exceptions;

public class AlreadyExistsException : EventException
{
    public AlreadyExistsException() : base("Resource already exists")
    {
    }

    public AlreadyExistsException(string message) : base(message)
    {
    }

    public AlreadyExistsException(string message, object key)
        : base($"Entity '{message}' ({key}) already exists.")
    {
    }
} 