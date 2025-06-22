namespace Raftlabs.Library.Exceptions;

/// <summary>
/// MissingDataException indicates bad requests.
/// </summary>
public class MissingDataException : RaftlabsException
{
    public MissingDataException()
    {
    }

    public MissingDataException(string message)
        : base(message)
    {
    }

    public MissingDataException(string message, Exception exception)
        : base(message, exception)
    {
    }
}