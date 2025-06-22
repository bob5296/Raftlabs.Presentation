namespace Raftlabs.Library.Exceptions;

public class NotFoundException : RaftlabsException
{
    public NotFoundException()
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string message, Exception exception)
        : base(message, exception)
    {
    }
}