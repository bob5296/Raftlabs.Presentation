namespace Raftlabs.Library.Exceptions;

public class DeserializationException : RaftlabsException
{
    public DeserializationException()
    {
    }

    public DeserializationException(string message)
        : base(message)
    {
    }

    public DeserializationException(string message, Exception exception)
        : base(message, exception)
    {
    }
}