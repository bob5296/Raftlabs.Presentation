namespace Raftlabs.Library.Exceptions;
public class BadGatewayException : RaftlabsException
{
    public BadGatewayException()
    {
    }

    public BadGatewayException(string message)
        : base(message)
    {
    }

    public BadGatewayException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }
}