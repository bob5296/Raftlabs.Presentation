namespace Raftlabs.Infra.Client.Models;

public class ApiClientResponseException : Exception
{
    public ApiClientResponseException(string message, Exception ex) : base(message, ex)
    {
    }
}