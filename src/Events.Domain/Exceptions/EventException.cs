namespace Events.Domain.Exceptions;

public class EventException : Exception
{
    public EventException()
    {
    }

    public EventException(string message) : base(message)
    {
    }

    public EventException(string message, Exception innerException) : base(message, innerException)
    {
    }
} 