namespace Raftlabs.Library.Exceptions;
public class GatewayTimeoutException : RaftlabsException
{
    public GatewayTimeoutException()
    {
    }

    public GatewayTimeoutException(string message)
        : base(message)
    {
    }

    public GatewayTimeoutException(string message, System.Exception innerException)
        : base(message, innerException)
    {
    }
}