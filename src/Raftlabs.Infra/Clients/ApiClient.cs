using System.Collections.Specialized;
using System.Net;
using Application.Abstractions;
using Raftlabs.Infra.Client.Models;
using Raftlabs.Library.Exceptions; // Assuming your custom exceptions are here

namespace Raftlabs.Infra.Client.Clients;

public class ApiClient : IApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HttpResponseMessage> GetAsync(string clientKey, string url, CancellationToken cancellationToken)
    {
        var request = GetRequest(HttpMethod.Get, url, null);
        request.Headers.Add("x-api-key", "reqres-free-v1");

        var client = _httpClientFactory.CreateClient(clientKey);
        var response = await client.SendAsync(request, cancellationToken);

        await HandleNonSuccessStatusCodeAsync(response);

        return response;
    }

    private static HttpRequestMessage GetRequest(HttpMethod verb, string url, NameValueCollection? headers)
    {
        var request = new HttpRequestMessage(verb, url);

        if (headers is not null)
        {
            foreach (string key in headers.AllKeys)
            {
                request.Headers.Add(key!, headers[key]);
            }
        }

        return request;
    }

    private static async Task HandleNonSuccessStatusCodeAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return;

        string responseContent = string.Empty;

        if (response.Content != null)
        {
            responseContent = await response.Content.ReadAsStringAsync();
        }

        if (response.StatusCode == HttpStatusCode.GatewayTimeout)
        {
            throw new GatewayTimeoutException(responseContent);
        }

        throw new BadGatewayException(responseContent);
    }
}
