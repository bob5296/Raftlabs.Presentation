using System.Net.Http.Headers;

namespace Application.Abstractions;

public interface IApiClient
{
    Task<HttpResponseMessage> GetAsync(string clientKey, string url,
        CancellationToken cancellationToken);
}
