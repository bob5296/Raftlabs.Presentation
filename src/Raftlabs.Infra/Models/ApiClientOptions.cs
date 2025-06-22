namespace Raftlabs.Infra.Client.Models;

public class ApiClientOptions
{
    public int RetryAttempts { get; init; } = 10;

    public int ExponentialBackoffMultipler { get; init; } = 2;

    public Client Client { get; init; }

    public void Validate()
    {
    }

}

public class Client
{
    public Client()
    {
        DefaultHeaders = new Dictionary<string, string>();
    }
    public string Url { get; init; }
    public string Key { get; set; }

    public Dictionary<string, string> DefaultHeaders { get; init; }

    public void Validate()
    {

    }
}
